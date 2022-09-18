using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace xor_encod
{
    class Program
    {
        public static class Globals
        {
            public static string curPath = System.IO.Directory.GetCurrentDirectory();
            public static byte[] bpayload = new byte[0];
        }
        public static void Main(string[] args)
        {
            string EncryptedPayload(byte[] payload)
            {
                //double encrypt xor
                for (int i = 0; i < payload.Length; i++)
                {
                    payload[i] = (byte)(((uint)payload[i] ^ 0xAA) & 0xFF);
                }

                StringBuilder haxe = new StringBuilder(payload.Length * 2);
                foreach (byte b in payload)
                {
                    haxe.AppendFormat("0x{0:x2}, ", b);
                }
                return haxe.ToString();
            }

            string ClearPayload(byte[] payload)
            {
                StringBuilder hax = new StringBuilder(payload.Length * 2);
                foreach (byte b in payload)
                {
                    hax.AppendFormat("0x{0:x2}, ", b);
                }
                return hax.ToString();
            }

            void PrintResults(string enc_in, string clear_in)
            {
                //encrypted result
                File.WriteAllText(Globals.curPath + "\\" + Convert.ToString(Globals.bpayload.Length) + "-enc-bytearray.txt", enc_in);
                Console.WriteLine("Payload written to: " + Globals.curPath + "\\" + Convert.ToString(Globals.bpayload.Length) + "-enc-bytearray.txt");
                Console.WriteLine("Length: " + Convert.ToString(Globals.bpayload.Length) + "\n");

                //clear result
                File.WriteAllText(Globals.curPath + "\\" + Convert.ToString(Globals.bpayload.Length) + "-bytearray.txt", clear_in);
                Console.WriteLine("Payload written to: " + Globals.curPath + "\\" + Convert.ToString(Globals.bpayload.Length) + "-bytearray.txt");
                Console.WriteLine("Length: " + Convert.ToString(Globals.bpayload.Length));
            }


            //Check args
            if (args.Length != 2)
            {
                Console.Clear();
                Console.WriteLine("Syntax: xor_encod.exe 1,2,3 payload, length(cs)");
                Console.WriteLine("1: b64 : xor_encod.exe 1 b64.txt");
                Console.WriteLine("2: raw : xor_encod.exe 3 met.bin");
                Console.WriteLine("3: cs  : xor_encod.exe 3 met.cs\n");
                Environment.Exit(0);
            }
            Console.Clear();

            //Read payload
            //b64
            if (Convert.ToInt32(args[0]) == 1)
            {
                Console.WriteLine("B64 Payload:");
                Globals.bpayload = Convert.FromBase64String(File.ReadAllText(Globals.curPath + "\\" + Convert.ToString(args[1])));
                string clearp = ClearPayload(Globals.bpayload);
                string encp = EncryptedPayload(Globals.bpayload);
                PrintResults(encp, clearp);
                Environment.Exit(0);
            }
            
            //raw
            if (Convert.ToInt32(args[0]) == 2)
            {
                Console.WriteLine("RAW Payload:");
                FileStream stream = File.OpenRead(Globals.curPath + "\\" + Convert.ToString(args[1]));
                byte[] fileBytes = new byte[stream.Length];
                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();
                Globals.bpayload = fileBytes;
                string clearp = ClearPayload(Globals.bpayload);
                string encp = EncryptedPayload(Globals.bpayload);
                PrintResults(encp, clearp);
                Environment.Exit(0);
            }

            //cs
            if (Convert.ToInt32(args[0]) == 3)
            {
                Console.WriteLine("CSHARP Payload:");
                Console.WriteLine("Remove the byte new buf and last comma");

                string s = File.ReadAllText(Globals.curPath + "\\" + Convert.ToString(args[1]));
                s = s.Replace("0x", "");
                s = s.Substring(0, s.Length - 2);
                s = s.Replace(" ", "");

                byte[] res =s.Split(",").Select((item) => Convert.ToByte(item, 16)).ToArray();
                Console.WriteLine(Convert.ToString(res.Length));
                Globals.bpayload = res;

                string clearp = ClearPayload(Globals.bpayload);
                string encp = EncryptedPayload(Globals.bpayload);
                PrintResults(encp, clearp);
                Environment.Exit(0);
            }

        }

    }


}
