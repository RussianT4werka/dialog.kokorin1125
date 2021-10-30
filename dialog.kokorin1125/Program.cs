using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dialog.kokorin1125
{
    class Program
    {
        static void Main(string[] args) 
        {
            int str = 0, sdv, m = 0, a = 0, a2 = 0;
            string path = "C:/Users/sanya/Desktop/dialog.tlk";
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))// открытие и использование файла                                                                        
            {
                reader.BaseStream.Seek(12, SeekOrigin.Current);
                str = reader.ReadInt32();            // считываем строки
                sdv = reader.ReadInt32();           // считываем сдвиг
                Console.WriteLine($"Количество строк:{str}, Cдвиг на ТРЕТЬЮ часть: {sdv} байт");
                Console.WriteLine("Нажмите Enter, чтобы увидеть кошмар эпилептика))))");
                Console.ReadKey();
                while (m < str) //ввожу цикл для чтения строк
                {
                      // (Первые 4 байта - всякие флаги, нам не нужны
                     //Еще 16 байт - звуковое сопровождение, не нужно. 
                    //Далее 8 байт - просто не используются, были зарезервированы, да не пригодились, так бывает. Итого 28 байт.
                    reader.BaseStream.Seek(28, SeekOrigin.Current); 

                    a = reader.ReadInt32();  //читаем сдвиг.
                    a2 = reader.ReadInt32(); //читаем длину строки.
                      //Следующие 4 байта - сдвиг относительно начала ТРЕТЕЙ части в файле к началу строки,
                     //о которой закодирована информация - тип int берем, пригодится.
                    //Задача - прочитать ТРЕТЬЮ часть. Поэтому пропускаем именно 4 байта.
                    reader.BaseStream.Seek(4, SeekOrigin.Current); 
                    long pos = reader.BaseStream.Position; // запоминаем позицию начала ТРЕТЬЕЙ части

                    reader.BaseStream.Seek(a + sdv, SeekOrigin.Begin); //находим позицию начала текстовой
                                                                         //строки относительно начала файла.
                    byte[] bytes = reader.ReadBytes(a2);  

                    Encoding unikod = Encoding.GetEncoding(0x4e3); // перекодировка.
                    string text = unikod.GetString(bytes);
                    reader.BaseStream.Seek(pos, SeekOrigin.Begin);//возвращаемся на позицию ТРЕТЬЕЙ части, которую запомнили ранее.
                    m++;
                    Console.WriteLine(text);  
                }
            }
        }
    }
}
