using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourExtraction
{
    public class Actions
    {
        private readonly Func<string> _inputProvider;
        private readonly Action<string> _outputProvider;
        private readonly YuvModel _yuv;
        private readonly Helpers _helpers;
        private string _outfilepath = "";


        public Actions(Func<string> inputProvider, Action<string> outputProvider, YuvModel yuv, Helpers helpers)
        {
            _inputProvider = inputProvider;
            _outputProvider = outputProvider;
            _yuv = yuv;
            _helpers = helpers;
        }



        public void GetInformation()
        {

            if (!File.Exists(Program.filepath))
            {
                Console.Clear();
                _outputProvider($"Could not find file '{Program.filepath}'.\n");
                Environment.Exit(-1);
            }


            _outputProvider("\n ┌─────────────────────────┐ ");
            _outputProvider("\n ▓   YUV File Components   ▓ ");
            _outputProvider("\n ▓          Y U V          ▓ ");
            _outputProvider("\n └─────────────────────────┘ \n\n");


            _outputProvider("  <Y component resolution>");
            _outputProvider("\n  width: "); var (Left, Top) = Console.GetCursorPosition();

            var result = _inputProvider();
            if (int.TryParse(result, out int value))
                _yuv.YWidth = value;

            _helpers.UserExit(result);

            _outputProvider("  height: ");
            result = _inputProvider();
            if (int.TryParse(result, out value))
                _yuv.YHeight = value;

            _helpers.UserExit(result);

            _outputProvider("\n  <U component resolution>");
            _outputProvider("\n  width: ");
            result = _inputProvider();
            if (int.TryParse(result, out value))
                _yuv.UWidth = value;

            _helpers.UserExit(result);

            _outputProvider("  height: ");
            result = _inputProvider();
            if (int.TryParse(result, out value))
                _yuv.UHeight = value;

            _helpers.UserExit(result);

            _outputProvider("\n  <V component resolution>");
            _outputProvider("\n  width: ");
            result = _inputProvider();
            if (int.TryParse(result, out value))
                _yuv.VWidth = value;

            _helpers.UserExit(result);

            _outputProvider("  height: ");
            result = _inputProvider();
            if (int.TryParse(result, out value))
                _yuv.VHeight = value;

            _helpers.UserExit(result);

            _yuv.YTotalBytes = _yuv.YResolution;
            _yuv.UTotalBytes = _yuv.UResolution;
            _yuv.VTotalBytes = _yuv.VResolution;



            Array.Resize(ref _yuv.Ybytes, _yuv.YResolution);
            Array.Resize(ref _yuv.Ubytes, _yuv.UResolution);
            Array.Resize(ref _yuv.Vbytes, _yuv.VResolution);


            FileProperties();
        }


        private void FileProperties()
        {
            _outputProvider("\n");
            _outputProvider("\n ┌──────────────────────┐ ");
            _outputProvider("\n ░   YUV File Details   ▓ ");
            _outputProvider("\n ▓        Y U V         ░ ");
            _outputProvider("\n └──────────────────────┘ ");
            _outputProvider("\n");
            _outputProvider($"\n  Name: {Path.GetFileName(Program.filepath)}");
            _outputProvider($"\n  Resolution: {_yuv.YWidth} x {_yuv.YHeight}");
            _outputProvider($"\n  Width: {_yuv.YWidth} pixels");
            _outputProvider($"\n  Height: {_yuv.YHeight} pixels");
            _outputProvider($"\n  Item Type: {Path.GetExtension(Program.filepath).ToUpper()} File");
            _outputProvider($"\n  Folder Path: {Path.GetDirectoryName(Program.filepath)}");
            _outputProvider($"\n  Date Created: {File.GetCreationTime(Program.filepath)}");
            _outputProvider($"\n  Date Modified: {File.GetLastAccessTime(Program.filepath)}");
            _outputProvider($"\n  Size: {_yuv.YTotalBytes} Bytes");
            _outputProvider($"\n  Owner: {Environment.UserName}");
            _outputProvider($"\n  Computer: {Environment.MachineName}");
        }


        public Task ReadFile()
        {
            try
            {
                using (FileStream fsSource = new(Program.filepath, FileMode.Open, FileAccess.Read))
                {
                    // Write y component into a byte array.
                    int numBytesRead = 0;
                    while (_yuv.YTotalBytes > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read(_yuv.Ybytes, numBytesRead, _yuv.YTotalBytes);

                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        _yuv.YTotalBytes -= n;
                    }
                    _yuv.YTotalBytes = _yuv.Ybytes.Length;


                    // Write u component into a byte array.
                    numBytesRead = 0;
                    while (_yuv.UTotalBytes > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read(_yuv.Ubytes, numBytesRead, _yuv.UTotalBytes);

                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        _yuv.UTotalBytes -= n;
                    }
                    _yuv.UTotalBytes = _yuv.Ubytes.Length;


                    // Write v component into a byte array.
                    numBytesRead = 0;
                    while (_yuv.VTotalBytes > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read(_yuv.Vbytes, numBytesRead, _yuv.VTotalBytes);

                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        _yuv.VTotalBytes -= n;
                    }
                    _yuv.VTotalBytes = _yuv.Vbytes.Length;



                    return Task.CompletedTask;
                }
            }
            catch (FileLoadException ioEx)
            {
                Console.Clear();
                Console.WriteLine($"{ioEx.Message}");

                Environment.Exit(-1);

                return Task.FromException(ioEx);
            }

        }

       
        public string CreateFilePath()
        {
            string path = $"{Environment.CurrentDirectory}/generated";


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory($"{Environment.CurrentDirectory}/generated");
                _outfilepath = $"{path}/{Path.GetFileNameWithoutExtension(Program.filepath)}_generated.yuv";
            }
            else
                _outfilepath = $"{Environment.CurrentDirectory}/generated/{Path.GetFileNameWithoutExtension(Program.filepath)}_generated.yuv";

            return _outfilepath;
        }


        public void WriteToFile()
        {

            try
            {

                // Write all component byte arrays to a new .yuv file.
                using (FileStream fsNew = new(_outfilepath, FileMode.Create, FileAccess.Write))
                {
                    for (int i = 0; i < _yuv.YBinary.GetLength(0); i++)
                    {
                        for (int j = 0; j < _yuv.YBinary.GetLength(1); j++)
                        {
                            fsNew.WriteByte(_yuv.YBinary[i, j]);
                        }
                    }

                    for (int i = 0; i < _yuv.Uplane.GetLength(0); i++)
                    {
                        for (int j = 0; j < _yuv.Uplane.GetLength(1); j++)
                        {
                            fsNew.WriteByte(_yuv.Uplane[i, j]);
                        }
                    }

                    for (int i = 0; i < _yuv.Vplane.GetLength(0); i++)
                    {
                        for (int j = 0; j < _yuv.Vplane.GetLength(1); j++)
                        {
                            fsNew.WriteByte(_yuv.Vplane[i, j]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _outputProvider(ex.ToString());
            }

        }



    }
}
