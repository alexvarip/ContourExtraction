
namespace ContourExtraction
{
    public class YuvModel
    {

        public int Mask { get; } = 3;
        public int YWidth { get; set; }
        public int YHeight { get; set; }
        public int UWidth { get; set; }
        public int UHeight { get; set; }
        public int VWidth { get; set; }
        public int VHeight { get; set; }
        

        /// <summary>
        /// Total bytes of Y component.
        /// </summary>
        public int YTotalBytes { get; set; }


        /// <summary>
        /// Total bytes of U component.
        /// </summary>
        public int UTotalBytes { get; set; }


        /// <summary>
        /// Total bytes of V component.
        /// </summary>
        public int VTotalBytes { get; set; }


        /// <summary>
        /// Stores the yuv image components.
        /// </summary>
        public byte[] Ybytes, Ubytes, Vbytes;


        /// <summary>
        /// Stores the yuv image components.
        /// </summary>
        public byte[,] Yplane, Uplane, Vplane;


        /// <summary>
        /// Stores the binary values (0/1 as 255) of the yuv image's y component.
        /// </summary>
        public byte[,] YBinary;


        /// <summary>
        /// Stores the binary values (0/1 as 255) of the yuv image's y component after erosion has been applied.
        /// </summary>
        public byte[,] YContour;


        /// <summary>
        /// Y component full resolution (width * height)
        /// </summary>
        public int YResolution { get { return YWidth * YHeight; } }


        /// <summary>
        /// U component full resolution (width * height)
        /// </summary>
        public int UResolution { get { return UWidth * UHeight; } }


        /// <summary>
        /// V component full resolution (width * height)
        /// </summary>
        public int VResolution { get { return VWidth * VHeight; } }


    }
}
