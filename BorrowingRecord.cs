using System;

namespace LibraryManagementSystem
{
    public class BorrowingRecord
    {
        public int MemberID { get; set; }
        public int BookID { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int OverdueDays { get; set; }
        public decimal PenaltyAmount { get; set; }

        public BorrowingStatus Status { get; set; }

        public BorrowingRecord(
            int memberID,
            int bookID,
            DateTime borrowDate)
        {
            MemberID = memberID;
            BookID = bookID;
            BorrowDate = borrowDate;
            DueDate = borrowDate.AddDays(7);
            ReturnDate = null;
            OverdueDays = 0;
            PenaltyAmount = 0m;
            Status = BorrowingStatus.Active;
        }
    }
}