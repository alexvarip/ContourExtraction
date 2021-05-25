
namespace ContourExtraction
{
    public class YuvModel
    {

        public int Mask { get; set; } = 3;
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
        /// For implementing read/wite operations for a .yuv file with one dimensional byte array.
        /// </summary>
        public byte[] Ybytes, Ubytes, Vbytes;


        /// <summary>
        /// For implementing border extension from a given one dimensional byte array to two dimensional byte array.
        /// </summary>
        public byte[,] Yplane, Uplane, Vplane;


        /// <summary>
        /// One dimensional byte array filled with median values.
        /// </summary>
        public byte[] YFiltered;


        /// <summary>
        /// Two dimensional array filled with the median values of a converted from one dimensional to two dimensional byte array.
        /// </summary>
        public byte[,] YFiltered2D;


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
