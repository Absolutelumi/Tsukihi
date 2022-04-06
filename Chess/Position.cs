namespace Tsukihi.Chess
{
    internal class Position
    {
        /// <summary>
        /// File of position <para/>
        /// Should be value between 0 - 7 <para/>
        /// 0 - 7 correlates to A - H on the board <para/>
        /// </summary>
        internal int File { get; set; }

        /// <summary>
        /// Rank of position <para/>
        /// Should be value between 0 - 7 <para/>
        /// 0 - 7 correaltes to 1 - 8 <para/>
        /// </summary>
        internal int Rank { get; set; }

        /// <summary>
        /// Define position at (file, rank) <para/>
        /// If file or rank value is less than 0, value will be set to 0 <para/>
        /// If file or rank value is greater than 7, value will be set to 7 <para/>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rank"></param>
        internal Position(int file, int rank)
        {
            File = file < 0 ?
                0 :
                file > 7 ?
                7 :
                file;

            Rank = rank < 0 ?
                0 :
                rank > 7 ?
                7 :
                rank;
        }
    }
}
