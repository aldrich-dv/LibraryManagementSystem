using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    // ENUMs
    public enum BookStatus
    {
        Available,
        Borrowed
    }

    public enum BorrowingStatus
    {
        Active,
        Returned
    }

    // variables
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public BookStatus Status { get; set; }

        public Book(int bookID, string title, string author, string isbn)
        {
            BookID = bookID;
            Title = title;
            Author = author;
            ISBN = isbn;
            Status = BookStatus.Available;
        }
    }

    public class Member
    {
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ContactDetails { get; set; }

        // original constructor
        public Member(int memberID, string name, string surname)
        {
            MemberID = memberID;
            Name = name;
            Surname = surname;
            ContactDetails = "";
        }

        // constructor with contact details
        public Member(int memberID, string name, string surname, string contactDetails)
        {
            MemberID = memberID;
            Name = name;
            Surname = surname;
            ContactDetails = contactDetails;
        }
    }

    // search and sort
    public class LibraryService
    {
        // lists
        public List<Book> Books { get; set; } = new List<Book>();
        public List<Member> Members { get; set; } = new List<Member>();
        public List<BorrowingRecord> BorrowingRecords { get; set; } = new List<BorrowingRecord>();

        // find book using BookID
        public Book? FindBookByID(int bookID)
        {
            foreach (Book book in Books)
            {
                if (book.BookID == bookID)
                {
                    return book;
                }
            }

            return null;
        }

        // find member using MemberID
        public Member? FindMemberByID(int memberID)
        {
            foreach (Member member in Members)
            {
                if (member.MemberID == memberID)
                {
                    return member;
                }
            }

            return null;
        }

        // add member
        public bool AddMember(int memberID, string name, string surname, string contactDetails)
        {
            if (memberID <= 0)
            {
                Console.WriteLine("Member ID must be greater than zero.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(contactDetails))
            {
                Console.WriteLine("Member details cannot be empty.");
                return false;
            }

            Member? existingMember = FindMemberByID(memberID);

            if (existingMember != null)
            {
                Console.WriteLine("A member with this ID already exists.");
                return false;
            }

            Member newMember = new Member(
                memberID,
                name.Trim(),
                surname.Trim(),
                contactDetails.Trim());

            Members.Add(newMember);

            Console.WriteLine("Member added successfully.");
            return true;
        }

        // update member
        public bool UpdateMember(int memberID, string name, string surname, string contactDetails)
        {
            Member? member = FindMemberByID(memberID);

            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(contactDetails))
            {
                Console.WriteLine("Member details cannot be empty.");
                return false;
            }

            member.Name = name.Trim();
            member.Surname = surname.Trim();
            member.ContactDetails = contactDetails.Trim();

            Console.WriteLine("Member updated successfully.");
            return true;
        }

        // remove member
        public bool RemoveMember(int memberID)
        {
            Member? member = FindMemberByID(memberID);

            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return false;
            }

            // prevent removing member with borrowed book
            foreach (BorrowingRecord record in BorrowingRecords)
            {
                if (record.MemberID == memberID &&
                    record.Status == BorrowingStatus.Active)
                {
                    Console.WriteLine(
                        "Member cannot be removed while they have a borrowed book.");

                    return false;
                }
            }

            Members.Remove(member);

            Console.WriteLine("Member removed successfully.");
            return true;
        }

        // borrow book
        public bool BorrowBook(int memberID, int bookID, DateTime borrowDate)
        {
            Member? member = FindMemberByID(memberID);

            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return false;
            }

            Book? book = FindBookByID(bookID);

            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return false;
            }

            // prevent book being borrowed more than once
            if (book.Status == BookStatus.Borrowed)
            {
                Console.WriteLine("This book is already borrowed.");
                return false;
            }

            BorrowingRecord record =
                new BorrowingRecord(memberID, bookID, borrowDate);

            BorrowingRecords.Add(record);

            // change book availability
            book.Status = BookStatus.Borrowed;

            Console.WriteLine("Book borrowed successfully.");
            Console.WriteLine($"Due date: {record.DueDate:dd MMMM yyyy}");

            return true;
        }

        // calculate overdue penalty
        public decimal CalculatePenalty(DateTime dueDate, DateTime returnDate)
        {
            if (returnDate.Date <= dueDate.Date)
            {
                return 0m;
            }

            int overdueDays = (returnDate.Date - dueDate.Date).Days;

            return overdueDays * 5m;
        }

        // return book
        public bool ReturnBook(int memberID, int bookID, DateTime returnDate)
        {
            Member? member = FindMemberByID(memberID);

            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return false;
            }

            Book? book = FindBookByID(bookID);

            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return false;
            }

            BorrowingRecord? activeRecord = null;

            // find active borrowing transaction
            foreach (BorrowingRecord record in BorrowingRecords)
            {
                if (record.MemberID == memberID &&
                    record.BookID == bookID &&
                    record.Status == BorrowingStatus.Active)
                {
                    activeRecord = record;
                    break;
                }
            }

            if (activeRecord == null)
            {
                Console.WriteLine("No active borrowing record was found.");
                return false;
            }

            if (returnDate.Date < activeRecord.BorrowDate.Date)
            {
                Console.WriteLine(
                    "Return date cannot be before the borrow date.");

                return false;
            }

            // calculate overdue days
            if (returnDate.Date > activeRecord.DueDate.Date)
            {
                activeRecord.OverdueDays =
                    (returnDate.Date - activeRecord.DueDate.Date).Days;
            }
            else
            {
                activeRecord.OverdueDays = 0;
            }

            activeRecord.ReturnDate = returnDate;

            activeRecord.PenaltyAmount =
                CalculatePenalty(activeRecord.DueDate, returnDate);

            activeRecord.Status = BorrowingStatus.Returned;

            // change book availability
            book.Status = BookStatus.Available;

            Console.WriteLine("Book returned successfully.");
            Console.WriteLine($"Overdue days: {activeRecord.OverdueDays}");
            Console.WriteLine(
                $"Penalty amount: R{activeRecord.PenaltyAmount:F2}");

            return true;
        }

        // search book
        public List<Book> SearchBooks(int choice, string searchTerm)
        {
            List<Book> results = new List<Book>();

            switch (choice)
            {
                case 1: // search by title
                    foreach (Book b in Books)
                    {
                        if (b.Title.ToLower().Contains(searchTerm.ToLower()))
                        {
                            results.Add(b);
                        }
                    }
                    break;

                case 2: // search by author
                    foreach (Book b in Books)
                    {
                        if (b.Author.ToLower().Contains(searchTerm.ToLower()))
                        {
                            results.Add(b);
                        }
                    }
                    break;

                case 3: // search by ISBN
                    foreach (Book b in Books)
                    {
                        if (b.ISBN.Equals(
                            searchTerm,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            results.Add(b);
                        }
                    }
                    break;

                case 4: // search by BookID
                    if (int.TryParse(searchTerm, out int id))
                    {
                        foreach (Book b in Books)
                        {
                            if (b.BookID == id)
                            {
                                results.Add(b);
                            }
                        }
                    }
                    break;

                default:
                    Console.WriteLine("Invalid search option selected.");
                    break;
            }

            return results;
        }

        // search member
        public List<Member> SearchMembers(int choice, string searchTerm)
        {
            List<Member> results = new List<Member>();

            switch (choice)
            {
                case 1: // search by name
                    foreach (Member m in Members)
                    {
                        if (m.Name.ToLower().Contains(searchTerm.ToLower()))
                        {
                            results.Add(m);
                        }
                    }
                    break;

                case 2: // search by surname
                    foreach (Member m in Members)
                    {
                        if (m.Surname.ToLower().Contains(searchTerm.ToLower()))
                        {
                            results.Add(m);
                        }
                    }
                    break;

                case 3: // search by MemberID
                    if (int.TryParse(searchTerm, out int id))
                    {
                        foreach (Member m in Members)
                        {
                            if (m.MemberID == id)
                            {
                                results.Add(m);
                            }
                        }
                    }
                    break;

                default:
                    Console.WriteLine("Invalid search option selected.");
                    break;
            }

            return results;
        }

        // sort book
        public void SortBooks(int choice, bool ascending)
        {
            switch (choice)
            {
                case 1: // sort by title
                    Books.Sort(
                        (a, b) => string.Compare(
                            a.Title,
                            b.Title,
                            StringComparison.OrdinalIgnoreCase));
                    break;

                case 2: // sort by author
                    Books.Sort(
                        (a, b) => string.Compare(
                            a.Author,
                            b.Author,
                            StringComparison.OrdinalIgnoreCase));
                    break;

                case 3: // sort by BookID
                    Books.Sort(
                        (a, b) => a.BookID.CompareTo(b.BookID));
                    break;

                case 4: // sort by status
                    Books.Sort(
                        (a, b) => a.Status.CompareTo(b.Status));
                    break;

                default:
                    Console.WriteLine("Invalid sort option selected.");
                    return;
            }

            if (!ascending)
            {
                Books.Reverse();
            }
        }

        // sort member
        public void SortMembers(int choice, bool ascending)
        {
            switch (choice)
            {
                case 1: // sort by name
                    Members.Sort(
                        (a, b) => string.Compare(
                            a.Name,
                            b.Name,
                            StringComparison.OrdinalIgnoreCase));
                    break;

                case 2: // sort by surname
                    Members.Sort(
                        (a, b) => string.Compare(
                            a.Surname,
                            b.Surname,
                            StringComparison.OrdinalIgnoreCase));
                    break;

                case 3: // sort by MemberID
                    Members.Sort(
                        (a, b) => a.MemberID.CompareTo(b.MemberID));
                    break;

                default:
                    Console.WriteLine("Invalid sort option selected.");
                    return;
            }

            if (!ascending)
            {
                Members.Reverse();
            }
        }

        // display methods
        public void DisplayBooks(List<Book> booksToDisplay)
        {
            if (booksToDisplay.Count == 0)
            {
                Console.WriteLine("No books found.");
                return;
            }

            foreach (Book b in booksToDisplay)
            {
                Console.WriteLine(
                    $"[{b.BookID}] {b.Title} by {b.Author} - " +
                    $"ISBN: {b.ISBN} - Status: {b.Status}");
            }
        }

        public void DisplayMembers(List<Member> membersToDisplay)
        {
            if (membersToDisplay.Count == 0)
            {
                Console.WriteLine("No members found.");
                return;
            }

            foreach (Member m in membersToDisplay)
            {
                Console.WriteLine(
                    $"[{m.MemberID}] {m.Name} {m.Surname} - " +
                    $"Contact: {m.ContactDetails}");
            }
        }

        // library statistics
        public void DisplayStatistics()
        {
            int totalBooks = Books.Count;
            int availableBooks = 0;
            int borrowedBooks = 0;

            foreach (Book book in Books)
            {
                if (book.Status == BookStatus.Available)
                {
                    availableBooks++;
                }
                else if (book.Status == BookStatus.Borrowed)
                {
                    borrowedBooks++;
                }
            }

            int totalMembers = Members.Count;
            int activeLoans = 0;
            int returnedBooks = 0;
            decimal totalPenalties = 0;

            foreach (BorrowingRecord record in BorrowingRecords)
            {
                if (record.Status == BorrowingStatus.Active)
                {
                    activeLoans++;
                }
                else if (record.Status == BorrowingStatus.Returned)
                {
                    returnedBooks++;
                }

                totalPenalties += record.PenaltyAmount;
            }

            Console.WriteLine("\n--- Library Statistics ---");
            Console.WriteLine($"Total books: {totalBooks}");
            Console.WriteLine($"Available books: {availableBooks}");
            Console.WriteLine($"Borrowed books: {borrowedBooks}\n");
            Console.WriteLine($"Total members: {totalMembers}\n");
            Console.WriteLine($"Active loans: {activeLoans}");
            Console.WriteLine($"Returned books: {returnedBooks}\n");
            Console.WriteLine($"Total penalties collected: R{totalPenalties:F2}");
        }

        // display overdue books report
        public void DisplayOverdueBooks()
        {
            bool foundOverdue = false;
            Console.WriteLine("\n--- Overdue Books Report ---");

            foreach (BorrowingRecord record in BorrowingRecords)
            {
                // only check active borrowed books
                if (record.Status == BorrowingStatus.Active && DateTime.Today > record.DueDate)
                {
                    foundOverdue = true;
                    Member? member = FindMemberByID(record.MemberID);
                    Book? book = FindBookByID(record.BookID);
                    int overdueDays = (DateTime.Today - record.DueDate).Days;
                    decimal penalty = overdueDays * 5m;

                    Console.WriteLine($"\nMember: {member?.Name} {member?.Surname}");
                    Console.WriteLine($"Book: {book?.Title}");
                    Console.WriteLine($"Due date: {record.DueDate:dd MMMM yyyy}");
                    Console.WriteLine($"Days overdue: {overdueDays}");
                    Console.WriteLine($"Current penalty: R{penalty:F2}");
                }
            }

            if (!foundOverdue)
            {
                Console.WriteLine("No overdue books found.");
            }
        }

        // display member borrowing history
        public void DisplayMemberHistory(int memberID)
        {
            Member? member = FindMemberByID(memberID);

            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return;
            }

            Console.WriteLine("\n--- Borrowing History ---");
            Console.WriteLine($"Member: {member.Name} {member.Surname}");
            bool foundHistory = false;

            foreach (BorrowingRecord record in BorrowingRecords)
            {
                if (record.MemberID == memberID)
                {
                    foundHistory = true;
                    Book? book = FindBookByID(record.BookID);

                    Console.WriteLine($"\nBook: {book?.Title}");
                    Console.WriteLine($"Borrowed: {record.BorrowDate:dd MMMM yyyy}");
                    Console.WriteLine($"Due date: {record.DueDate:dd MMMM yyyy}");

                    if (record.Status == BorrowingStatus.Returned)
                    {
                        Console.WriteLine($"Returned: {record.ReturnDate:dd MMMM yyyy}");
                        Console.WriteLine($"Penalty: R{record.PenaltyAmount:F2}");
                    }
                    else
                    {
                        Console.WriteLine("Status: Currently borrowed");
                    }
                }
            }

            if (!foundHistory)
            {
                Console.WriteLine("No borrowing history found.");
            }
        }
    }
}