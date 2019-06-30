using System;
using System.Net;
using System.Net.Sockets;
using Audio.Codec;
using System.Threading;
using NAudio.Wave;

namespace Audio.VoipCall
{
    public class VoipCall
    {
        #region Attributes

        //UDP
        UdpClient udpSender;
        UdpClient udpReceiver;

        //IP & Port
        string destinationIP;
        int port;
        IPEndPoint sourceIPEP;

        //Naudio
        WaveIn waveSource = null;
        WaveFileWriter waveFile_sent = null;
        WaveFileWriter waveFile_received = null;
        WaveOut waveOut;
        BufferedWaveProvider waveProvider;

        Thread ReceiverThread = null;

        #endregion

        /// <summary>
        /// Initialize UDP's Sockets IPAddress and Port
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public VoipCall(string ipAddress, int port)
        {
            this.port = port;
            this.destinationIP = ipAddress;
            sourceIPEP = new IPEndPoint(IPAddress.Any, port);
        }

        /// <summary>
        /// Initialize and Run Voice Call
        /// </summary>
        public void InitVoiceCall()
        {
            Console.WriteLine("Voice Call Started ...");

            //init UDP Socket
            udpSender = new UdpClient(destinationIP, port);

            //Init Mic
            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(44100, 1);
            waveSource.BufferMilliseconds = 50;

            //Mic Events
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            waveFile_sent = new WaveFileWriter(@"0_send.wav", waveSource.WaveFormat);

            waveSource.StartRecording();

            ReceiverThread = new Thread(new ThreadStart(StartReceiving));
            ReceiverThread.Start();

        }
        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile_sent != null)
            {
                //Save to file:
                waveFile_sent.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile_sent.Flush();

                //Send via UDP:
                byte[] encoded = MuLawCodec.Encode(e.Buffer, 0, e.BytesRecorded);
                udpSender.Send(encoded, encoded.Length);
            }
        }
        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile_sent != null)
            {
                waveFile_sent.Dispose();
                waveFile_sent = null;
            }
        }

        void StartReceiving()
        {
            udpReceiver = new UdpClient(sourceIPEP);

            //init Speakers
            waveOut = new WaveOut();
            waveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
            waveOut.Init(waveProvider);
            waveOut.Play();

            waveFile_received = new WaveFileWriter(@"1_receive.wav", new WaveFormat(44100, 1));

            while (true)
            {
                try
                {
                    //Decode received buffer
                    byte[] data = udpReceiver.Receive(ref sourceIPEP);
                    byte[] decoded = MuLawCodec.Decode(data, 0, data.Length);

                    //Save it to the file
                    waveFile_received.Write(decoded, 0, decoded.Length);
                    waveFile_received.Flush();

                    //Add to waveProvider Buffer to Play the Sound
                    waveProvider.AddSamples(decoded, 0, decoded.Length);
                }
                catch (Exception)
                {
                    Console.WriteLine("Voice Call Dropped !");
                    break;
                }
            }
        }

        /// <summary>
        /// Drop the initialized Call (Closed !)
        /// </summary>
        public void DropCall()
        {
            //Stop Mic
            waveSource.StopRecording();
            
            //Dispose FileStream Writer
            waveFile_received.Dispose();
            
            //Stop Speakers
            waveOut.Stop();
            
            //Close Connections
            udpSender.Close();
            udpReceiver.Close();
        }
    }
}
