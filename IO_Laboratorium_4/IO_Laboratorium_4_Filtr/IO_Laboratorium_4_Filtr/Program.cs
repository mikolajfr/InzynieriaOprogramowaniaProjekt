using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace IO_Laboratorium_4_Filtr
{
    
    class Filtrowanie
    {
        public static byte[][] obraz;
        public static byte[][] obszar_roboczy;
        public static byte[][] filtr;
        
        public static void ustawWymiaryObrazu(int w, int h)
        {
            obraz = new byte[w][];
            for(int i=0; i< w;i++)
            {
                obraz[i] = new byte[h];
            }
        }
        public static void ustawWymiaryFiltra(int w, int h)
        {
            filtr = new byte[w][];
            for (int i = 0; i < filtr.Length; i++)
            {
                filtr[i] = new byte[h];
            }

            // Dodatkowo zostaje utworzony powiekszony obszar roboczy
            int powiekszony_obszar = w-1;
            obszar_roboczy = new byte[obraz.Length + powiekszony_obszar][];
            for(int i=0; i< obszar_roboczy.Length;i++)
            {
                obszar_roboczy[i] = new byte[obraz[0].Length + powiekszony_obszar];
            }

        }
        // Wczytanie obrazu do tablicy obraz oraz obszaru roboczego
        public static void wczytajObraz(String Path)
        {
            Image img = Image.FromFile(Path);
            if (img.Width != obraz.Length || img.Height != obraz[0].Length)
                return;

            byte[] temp_tab;

            using (MemoryStream mem = new MemoryStream())
            {
                img.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                temp_tab = mem.ToArray();
            }

            for (int x = 0; x < obraz.Length; x++)
            {
                for (int y = 0; y < obraz[0].Length; y++)
                {
                    obraz[y][x] = temp_tab[x * img.Width + y];
                    obszar_roboczy[y + 1][x + 1] = temp_tab[x * img.Width + y];
                }
            }
        }
        public static void wczytajFiltr(byte[][] filtr_2)
        {
            if (filtr_2.Length != filtr.Length || filtr_2[0].Length != filtr[0].Length)
                return;
            for (int x = 0; x < filtr.Length; x++)
            {
                for (int y = 0; y < filtr[0].Length; y++)
                {
                    filtr[y][x] = filtr_2[x][y];
                }
            }
        }

        public static void wyswietlTablice(byte[][] tab)
        {
            for(int i = 0; i < tab.Length; i++)
            {
                String wiersz = "[";
                bool pierwszy = true;
                for(int j=0; j < tab[0].Length; j++)
                {
                    if (pierwszy)
                        pierwszy = false;
                    else
                        wiersz += " ";
                    wiersz += tab[i][j];
                }
                wiersz += "]";
                Console.WriteLine(wiersz);
            }
        }

        public static void sumaPikseli(byte[][] tab)
        {
            int suma = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab[0].Length; j++)
                {
                    suma += tab[i][j];
                }
            }
            Console.WriteLine("Suma wartosci pikseli: {0}", suma);
        }

        public static void wlaczFiltrowanie()
        {
            for(int x=1;x<obszar_roboczy.Length - 1; x++)
            {
                for(int y=1; y < obszar_roboczy[0].Length - 1; y++)
                {
                    int temp = 0;
                    byte K = 0;

                    for (int i = 0; i < 9; i++)
                    {
                        temp += obszar_roboczy[y - 1 + i % 3][x - 1 + (i / 3)] * filtr[i % 3][i / 3];                
                        K += filtr[i % 3][i / 3];
                    }

                    if (K == 0) K = 1;
                    obraz[y - 1][x - 1] = (byte)(temp / K);
                }
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            byte[][] filtr = new byte[][]
            {
                new byte[]{1, 2, 1},
                new byte[]{2, 1, 2},
                new byte[]{1, 2, 1}
            };
            Filtrowanie.ustawWymiaryObrazu(100, 100);
            Filtrowanie.ustawWymiaryFiltra(3, 3);
            Filtrowanie.wczytajObraz("eye.jpg");
            Filtrowanie.wczytajFiltr(filtr);
            Console.WriteLine("Tablica bajtów obrazu przed działaniem filtra:");
            Filtrowanie.wyswietlTablice(Filtrowanie.obraz);
            Filtrowanie.sumaPikseli(Filtrowanie.obraz);

            Filtrowanie.wlaczFiltrowanie();
            Console.WriteLine("\nTablica bajtów obrazu po działaniu filtra:");
            Filtrowanie.wyswietlTablice(Filtrowanie.obraz);
            Filtrowanie.sumaPikseli(Filtrowanie.obraz);

            Console.WriteLine("Zakończono wykonywanie programu!");
            Console.ReadKey();
        }
    }
}
