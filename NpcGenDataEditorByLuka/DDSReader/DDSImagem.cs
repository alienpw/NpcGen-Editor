﻿using Pfim;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

namespace pwAdminLocal.DDSReader
{
    public class DDSImagem
    {
        private readonly Pfim.IImage _image;

        public byte[] Data
        {
            get
            {
                if (_image != null)
                    return _image.Data;
                else
                    return new byte[0];
            }
        }

        public DDSImagem(string file)
        {
            _image = Pfim.Pfimage.FromFile(file);
            Process();
        }

        public DDSImagem(Stream stream)
        {
            if (stream == null)
                throw new Exception("DDSImage ctor: Stream is null");

            _image = Pfim.Dds.Create(stream, new Pfim.PfimConfig());
            Process();
        }

        public DDSImagem(byte[] data)
        {
            if (data == null || data.Length <= 0)
                throw new Exception("DDSImage ctor: no data");

            _image = Pfim.Dds.Create(data, new Pfim.PfimConfig());
            Process();
        }

        public void Save(string file)
        {
            if (_image.Format == Pfim.ImageFormat.Rgba32)
                Save<Bgra32>(file);
            else if (_image.Format == Pfim.ImageFormat.Rgb24)
                Save<Bgr24>(file);
            else
                throw new Exception("Unsupported pixel format (" + _image.Format + ")");
        }

        private void Process()
        {
            if (_image == null)
                throw new Exception("DDSImage image creation failed");

            if (_image.Compressed)
                _image.Decompress();
        }

        private void Save<T>(string file)
        where T : unmanaged, IPixel<T>
        {
            Image<T> image = Image.LoadPixelData<T>(
                _image.Data, _image.Width, _image.Height);
            image.Save(file);
        }

    }
}
