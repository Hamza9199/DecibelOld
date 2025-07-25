using NAudio.Wave;

namespace Decibel.Services
{
        public class AudioServis
        {
                public static double GetAudioTrajanje(Stream audioStream)
                {
                        using (var reader = new Mp3FileReader(audioStream))
                        {
                                return reader.TotalTime.TotalSeconds;
                        }
                }
        }
}
