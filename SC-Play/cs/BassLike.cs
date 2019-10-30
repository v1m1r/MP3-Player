using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;
namespace SC_Play.cs
{
    public static class BassLike
    {
        private static int HZ = 44100;  //Частота дискридитации
        public static bool InitDefaultDevice; //Состояние инициализации

        public static int Stream;//Канал-поток

        public static int Volume = 100;//Громкость

        private static bool isStoped = true;//канал остановлен руками

        public static bool EndPlayList;




        //Инициализация Bass.dll
        private static bool InitBass(int hz)
        {
            if (!InitDefaultDevice)
                InitDefaultDevice = Bass.BASS_Init(-1, hz, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            return InitDefaultDevice;
            
        }

        //Воспроизведение
        public static void Play(string filename, int vol)

        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();
                if (InitBass(HZ))
                {
                    Stream = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT);
                    if (Stream != 0)
                    {
                        Volume = vol;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);

                    }

                }
            }

            else Bass.BASS_ChannelPlay(Stream, false);
            isStoped = false;
        }

        //Кнопка СТОП
        public static void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
            isStoped = true;
           
        
        }

        //Кнопка пауза

        public static void Pause()
        {
            if (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(Stream);

        
        
        }




        //Получим длительность трека
        public static int GetTimeOfStream(int stream)
        {

            long TimeBytes = Bass.BASS_ChannelGetLength(stream);
            double Time = Bass.BASS_ChannelBytes2Seconds(stream, TimeBytes);
            return (int)Time;
        
        }


        //Текущая позиция
        public static int GetPosOfStream(int stream)
        {
            long pos = Bass.BASS_ChannelGetPosition(stream);
            int posSec = (int)Bass.BASS_ChannelBytes2Seconds(stream, pos);
            return posSec;
        }

        //Перемотка трека
        public static void SetPosOfScroll(int stream, int pos)
        {
            Bass.BASS_ChannelSetPosition(stream, (double)pos);
        
        }

        //Установка громкости
        public static void SetVolumeToStream(int stream, int vol)
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);

        
        }

        //Переключение треков
        public static bool ToNextTrack()

        {

            if ((Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_STOPPED) && (!isStoped))
            {
                if (Vars.Files.Count > Vars.CurrentTrackNumber +1)
                {
                    Play(Vars.Files[++Vars.CurrentTrackNumber], Volume);
                    EndPlayList = false;
                    return true;
                }
                else EndPlayList = true;
            
            }
            return false;
        
        }
        
        }


    }
