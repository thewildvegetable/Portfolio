using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            //get current directory
            DirectoryInfo di = new DirectoryInfo(".");

            Console.WriteLine("CURRENT DIR: " + di.FullName);

            String path = di.FullName;
            if (path.Contains("FileChecker"))
            {
                path = "/../../../../GameObject/Content/shopInventory.bin";
            }
            else
            {
                path = "/../../../Content/shopInventory.bin";
            }
            Console.WriteLine(di.FullName + path);

            //check if file exists
            if (File.Exists(di.FullName + path))
            {
                //check if file was edited
                if (File.GetCreationTime(di.FullName + path) != File.GetLastWriteTime(di.FullName + path))
                {
                    //rewrite file
                    Stream fs = File.OpenWrite(di.FullName + path);
                    BinaryWriter output = new BinaryWriter(fs);

                    //write the orange
                    output.Write("Orange");
                    output.Write("Yarr! Ye don't want scurvy do ye?");
                    output.Write(100);
                    output.Write(100);
                    output.Write(20);
                    output.Write(0);

                    //write the oil barrel
                    output.Write("Oil Barrel");
                    output.Write("Barrels for storing your fuel");
                    output.Write(100);
                    output.Write(100);
                    output.Write(70);
                    output.Write(0);

                    output.Close();

                    //change creation date to be the same as the last time edited
                    File.SetCreationTime(di.FullName + path, File.GetLastWriteTime(di.FullName + path));
                }
            }
            else
            {
                //create file
                Stream fs = File.OpenWrite(di.FullName + path);
                BinaryWriter output = new BinaryWriter(fs);

                //write the orange
                output.Write("Orange");
                output.Write("Yarr! Ye don't want scurvy do ye?");
                output.Write(100);
                output.Write(100);
                output.Write(20);
                output.Write(0);

                //write the oil barrel
                output.Write("Oil Barrel");
                output.Write("Barrels for storing your fuel");
                output.Write(100);
                output.Write(100);
                output.Write(70);
                output.Write(0);

                output.Close();
            }
            
        }
    }
}
