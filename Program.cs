using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    class Program
    {
        // overdue report
        static void OverdueReport()
        {
            library.DisplayOverdueBooks();
        }

        // member history
        static void MemberHistory()
        {
            Console.WriteLine("\n--- Member Borrowing History ---");

            Console.Write("Enter Member ID: ");

            if (int.TryParse(Console.ReadLine(),out int memberID))
            {
                library.DisplayMemberHistory(memberID);
            }
            else
            {
                Console.WriteLine("Invalid Member ID.");
            }
        }

        static LibraryService library = new LibraryService();

        static void Main(string[] args)
        {
            try
            {
                testdata();
                int mainChoice = 0;

                do
                {
                    Console.WriteLine("\n--- Library Management System ---");
                    Console.WriteLine("1 = Manage Books");
                    Console.WriteLine("2 = Manage Members");
                    Console.WriteLine("3 = Borrow Book");
                    Console.WriteLine("4 = Return Book");
                    Console.WriteLine("5 = View Statistics");
                    Console.WriteLine("6 = Overdue Books Report");
                    Console.WriteLine("7 = Member Borrowing History");
                    Console.WriteLine("8 = Exit");
                    Console.Write("Enter a number between 1 and 8: ");

                    if (int.TryParse(Console.ReadLine(), out mainChoice))
                    {
                        switch (mainChoice)
                        {
                            case 1:
                                ManageBooks();
                                break;

                            case 2:
                                ManageMembers();
                                break;

                            case 3:
                                BorrowBook();
                                break;

                            case 4:
                                ReturnBook();
                                break;

                            case 5:
                                library.DisplayStatistics();
                                break;

                            case 6:
                                OverdueReport();
                                break;

                            case 7:
                                MemberHistory();
                                break;

                            case 8:
                                Console.WriteLine("Goodbye.");
                                break;

                            default:
                                Console.WriteLine("Invalid option.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid option.");
                    }

                } while (mainChoice != 8);
            }
            catch
            {
                Console.WriteLine("An unexpected error occurred");
            } 
        }

        // book menu
        static void ManageBooks()
        {
            int bookChoice = 0;

            do
            {
                Console.WriteLine("\n--- Manage Books ---");
                Console.WriteLine("1 = Search books");
                Console.WriteLine("2 = Sort books");
                Console.WriteLine("3 = Display all books");
                Console.WriteLine("4 = Return to main menu");
                Console.Write("Enter a number between 1 and 4: ");

                if (int.TryParse(Console.ReadLine(), out bookChoice))
                {
                    switch (bookChoice)
                    {
                        case 1:
                            {
                                Console.WriteLine(
                                    "Search by: 1 = Title, 2 = Author, " +
                                    "3 = ISBN, 4 = BookID");

                                if (!int.TryParse(
                                    Console.ReadLine(),
                                    out int searchOption))
                                {
                                    Console.WriteLine(
                                        "Invalid search option.");

                                    break;
                                }

                                Console.Write("Enter search term: ");

                                string searchTerm =
                                    Console.ReadLine() ?? string.Empty;

                                if (string.IsNullOrWhiteSpace(searchTerm))
                                {
                                    Console.WriteLine(
                                        "Search term cannot be empty.");

                                    break;
                                }

                                List<Book> results =
                                    library.SearchBooks(
                                        searchOption,
                                        searchTerm);

                                library.DisplayBooks(results);
                                break;
                            }

                        case 2:
                            {
                                Console.WriteLine(
                                    "Sort by: 1 = Title, 2 = Author, " +
                                    "3 = BookID, 4 = Status");

                                if (!int.TryParse(
                                    Console.ReadLine(),
                                    out int sortOption))
                                {
                                    Console.WriteLine(
                                        "Invalid sort option.");

                                    break;
                                }

                                Console.Write("Ascending? (y/n): ");

                                bool ascending =
                                    (Console.ReadLine() ?? "y")
                                    .ToLower() == "y";

                                library.SortBooks(sortOption, ascending);

                                Console.WriteLine("Books sorted.");
                                break;
                            }

                        case 3:
                            library.DisplayBooks(library.Books);
                            break;

                        case 4:
                            Console.WriteLine(
                                "Returning to main menu.");
                            break;

                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }

            } while (bookChoice != 4);
        }

        // member menu
        static void ManageMembers()
        {
            int memberChoice = 0;

            do
            {
                Console.WriteLine("\n--- Manage Members ---");
                Console.WriteLine("1 = Add member");
                Console.WriteLine("2 = Update member");
                Console.WriteLine("3 = Remove member");
                Console.WriteLine("4 = Search members");
                Console.WriteLine("5 = Sort members");
                Console.WriteLine("6 = Display all members");
                Console.WriteLine("7 = Return to main menu");
                Console.Write("Enter a number between 1 and 7: ");

                if (int.TryParse(
                    Console.ReadLine(),
                    out memberChoice))
                {
                    switch (memberChoice)
                    {
                        case 1:
                            AddMember();
                            break;

                        case 2:
                            UpdateMember();
                            break;

                        case 3:
                            RemoveMember();
                            break;

                        case 4:
                            {
                                Console.WriteLine(
                                    "Search by: 1 = Name, " +
                                    "2 = Surname, 3 = MemberID");

                                if (!int.TryParse(
                                    Console.ReadLine(),
                                    out int searchOption))
                                {
                                    Console.WriteLine(
                                        "Invalid search option.");

                                    break;
                                }

                                Console.Write("Enter search term: ");

                                string searchTerm =
                                    Console.ReadLine() ?? string.Empty;

                                if (string.IsNullOrWhiteSpace(searchTerm))
                                {
                                    Console.WriteLine(
                                        "Search term cannot be empty.");

                                    break;
                                }

                                List<Member> results =
                                    library.SearchMembers(
                                        searchOption,
                                        searchTerm);

                                library.DisplayMembers(results);
                                break;
                            }

                        case 5:
                            {
                                Console.WriteLine(
                                    "Sort by: 1 = Name, " +
                                    "2 = Surname, 3 = MemberID");

                                if (!int.TryParse(
                                    Console.ReadLine(),
                                    out int sortOption))
                                {
                                    Console.WriteLine(
                                        "Invalid sort option.");

                                    break;
                                }

                                Console.Write("Ascending? (y/n): ");

                                bool ascending =
                                    (Console.ReadLine() ?? "y")
                                    .ToLower() == "y";

                                library.SortMembers(sortOption, ascending);

                                Console.WriteLine("Members sorted.");
                                break;
                            }

                        case 6:
                            library.DisplayMembers(library.Members);
                            break;

                        case 7:
                            Console.WriteLine(
                                "Returning to main menu.");
                            break;

                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }

            } while (memberChoice != 7);
        }

        // add member
        static void AddMember()
        {
            Console.WriteLine("\n--- Add Member ---");

            Console.Write("Enter Member ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int memberID))
            {
                Console.WriteLine("Invalid Member ID.");
                return;
            }

            Console.Write("Enter name: ");
            string name =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Enter surname: ");
            string surname =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Enter contact details: ");
            string contactDetails =
                Console.ReadLine() ?? string.Empty;

            library.AddMember(
                memberID,
                name,
                surname,
                contactDetails);
        }

        // update member
        static void UpdateMember()
        {
            Console.WriteLine("\n--- Update Member ---");

            Console.Write("Enter Member ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int memberID))
            {
                Console.WriteLine("Invalid Member ID.");
                return;
            }

            Console.Write("Enter new name: ");
            string name =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Enter new surname: ");
            string surname =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Enter new contact details: ");
            string contactDetails =
                Console.ReadLine() ?? string.Empty;

            library.UpdateMember(
                memberID,
                name,
                surname,
                contactDetails);
        }

        // remove member
        static void RemoveMember()
        {
            Console.WriteLine("\n--- Remove Member ---");

            Console.Write("Enter Member ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int memberID))
            {
                Console.WriteLine("Invalid Member ID.");
                return;
            }

            library.RemoveMember(memberID);
        }

        // borrow book
        static void BorrowBook()
        {
            Console.WriteLine("\n--- Borrow Book ---");

            Console.Write("Enter Member ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int memberID))
            {
                Console.WriteLine("Invalid Member ID.");
                return;
            }

            Console.Write("Enter Book ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int bookID))
            {
                Console.WriteLine("Invalid Book ID.");
                return;
            }

            library.BorrowBook(
                memberID,
                bookID,
                DateTime.Today);
        }

        // return book
        static void ReturnBook()
        {
            Console.WriteLine("\n--- Return Book ---");

            Console.Write("Enter Member ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int memberID))
            {
                Console.WriteLine("Invalid Member ID.");
                return;
            }

            Console.Write("Enter Book ID: ");

            if (!int.TryParse(
                Console.ReadLine(),
                out int bookID))
            {
                Console.WriteLine("Invalid Book ID.");
                return;
            }

            library.ReturnBook(
                memberID,
                bookID,
                DateTime.Today);
        }

        // temp data, delete later
        static void testdata()
        {
            library.Books.Add(
                new Book(
                    1,
                    "The Hobbit",
                    "J.R.R. Tolkien",
                    "9780345339683"));

            library.Books.Add(
                new Book(
                    2,
                    "Dune",
                    "Frank Herbert",
                    "9780441013593"));

            library.Books.Add(
                new Book(
                    3,
                    "Harry Potter",
                    "J.K. Rowling",
                    "9780439708180"));

            library.Members.Add(
                new Member(
                    1,
                    "James",
                    "Smith",
                    "0712345678"));

            library.Members.Add(
                new Member(
                    2,
                    "Sarah",                    "Johnson",
                    "0723456789"));

            library.Members.Add(
                new Member(
                    3,
                    "Michael",
                    "Miller",
                    "0734567890"));
        }
    }
}