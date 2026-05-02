namespace Library.Infrastructure.Persistence.Seed
{
    public static class SeedData
    {
        public record AuthorData(string Name, string Description);
        public record PublisherData(string Name);
        public record BookData(
            string Title,
            string AuthorName,
            string PublisherName,
            string ISBN,
            string Description,
            int Pages,
            int Year,
            CoverType CoverType,
            Language Language,
            int TotalCopies,
            string[] GenreNames
        );

        public static readonly List<AuthorData> Authors =
        [
            new("J.R.R. Tolkien", "English writer, poet, philologist and academic, best known for The Hobbit and The Lord of the Rings."),
            new("J.K. Rowling", "British author and philanthropist, best known for the Harry Potter fantasy series."),
            new("George R.R. Martin", "American novelist and short story writer known for A Song of Ice and Fire series."),
            new("Stephen King", "American author of horror, supernatural fiction, suspense, crime and fantasy novels."),
            new("Agatha Christie", "English writer known for her sixty-six detective novels and fourteen short story collections."),
            new("Ernest Hemingway", "American novelist, short story writer and journalist, Nobel Prize winner in 1954."),
            new("Gabriel García Márquez", "Colombian novelist, short-story writer, Nobel Prize winner, known for magical realism."),
            new("Leo Tolstoy", "Russian writer regarded as one of the greatest novelists of all time."),
            new("Fyodor Dostoevsky", "Russian novelist and philosopher, one of the greatest writers of all time."),
            new("George Orwell", "English novelist, essayist and critic, best known for 1984 and Animal Farm."),
            new("Aldous Huxley", "English writer and philosopher, best known for Brave New World."),
            new("Ray Bradbury", "American author of science fiction, horror, mystery and fantasy."),
            new("Isaac Asimov", "American author and professor of biochemistry, known for science fiction works."),
            new("Arthur Conan Doyle", "British writer best known for Sherlock Holmes detective stories."),
            new("Dan Brown", "American author best known for thriller novels including The Da Vinci Code."),
            new("Paulo Coelho", "Brazilian lyricist and novelist, author of The Alchemist."),
            new("Haruki Murakami", "Japanese writer known for surrealist fiction blending Western and Japanese culture."),
            new("Frank Herbert", "American science fiction author, best known for the Dune series."),
            new("Douglas Adams", "English author and satirist, known for The Hitchhiker's Guide to the Galaxy."),
            new("Иван Вазов", "Български писател, поет и общественик, автор на романа Под игото."),
            new("Алеко Константинов", "Български писател и публицист, автор на сатиричните разкази Бай Ганьо."),
            new("Йордан Йовков", "Български писател и драматург, автор на Старопланински легенди."),
            new("Елин Пелин", "Български писател, автор на хумористични разкази за живота в селото."),
        ];

        public static readonly List<PublisherData> Publishers =
        [
            new("HarperCollins"),
            new("Bloomsbury"),
            new("Penguin Random House"),
            new("Simon & Schuster"),
            new("Macmillan Publishers"),
            new("Scholastic"),
            new("Tor Books"),
            new("Doubleday"),
            new("Hodder & Stoughton"),
            new("Ciela"),
            new("Colibri"),
            new("Janet 45"),
            new("Hermes"),
            new("Enthusiast"),
        ];

        public static readonly List<BookData> Books =
        [
            // J.R.R. Tolkien
            new("The Hobbit", "J.R.R. Tolkien", "HarperCollins", "9780261102217",
                "A hobbit named Bilbo Baggins embarks on an unexpected journey with dwarves and the wizard Gandalf to reclaim the Lonely Mountain.",
                310, 1937, CoverType.Softcover, Language.English, 5, ["Fantasy", "Children"]),

            new("The Fellowship of the Ring", "J.R.R. Tolkien", "HarperCollins", "9780261102354",
                "Frodo Baggins inherits the One Ring and sets out on a perilous quest to destroy it in the fires of Mount Doom.",
                423, 1954, CoverType.Hardcover, Language.English, 4, ["Fantasy", "World Classics"]),

            new("The Two Towers", "J.R.R. Tolkien", "HarperCollins", "9780261102361",
                "The Fellowship is broken. Frodo and Sam continue toward Mordor while Aragorn leads the fight against Saruman.",
                352, 1954, CoverType.Hardcover, Language.English, 4, ["Fantasy", "World Classics"]),

            new("The Return of the King", "J.R.R. Tolkien", "HarperCollins", "9780261102378",
                "The final battle against Sauron is fought as Frodo and Sam reach the very heart of Mordor.",
                416, 1955, CoverType.Hardcover, Language.English, 4, ["Fantasy", "World Classics"]),
 
            // J.K. Rowling
            new("Harry Potter and the Philosopher's Stone", "J.K. Rowling", "Bloomsbury", "9780747532699",
                "A young boy discovers he is a wizard and begins his education at Hogwarts School of Witchcraft and Wizardry.",
                223, 1997, CoverType.Softcover, Language.English, 6, ["Fantasy", "Children"]),

            new("Harry Potter and the Chamber of Secrets", "J.K. Rowling", "Bloomsbury", "9780747538486",
                "Harry's second year at Hogwarts is plagued by mysterious attacks and a voice only he can hear.",
                251, 1998, CoverType.Softcover, Language.English, 5, ["Fantasy", "Children"]),

            new("Harry Potter and the Prisoner of Azkaban", "J.K. Rowling", "Bloomsbury", "9780747542155",
                "A dangerous prisoner has escaped from Azkaban and is believed to be hunting Harry Potter.",
                317, 1999, CoverType.Softcover, Language.English, 5, ["Fantasy", "Children"]),

            new("Harry Potter and the Goblet of Fire", "J.K. Rowling", "Bloomsbury", "9780747546245",
                "Harry is mysteriously entered into the deadly Triwizard Tournament against his will.",
                636, 2000, CoverType.Hardcover, Language.English, 4, ["Fantasy", "Children"]),

            new("Harry Potter and the Order of the Phoenix", "J.K. Rowling", "Bloomsbury", "9780747551003",
                "Harry forms Dumbledore's Army to fight against the Ministry's refusal to acknowledge Voldemort's return.",
                766, 2003, CoverType.Hardcover, Language.English, 4, ["Fantasy", "Children"]),

            new("Harry Potter and the Half-Blood Prince", "J.K. Rowling", "Bloomsbury", "9780747581086",
                "Dumbledore prepares Harry for the final confrontation as dark secrets about Voldemort are revealed.",
                607, 2005, CoverType.Hardcover, Language.English, 4, ["Fantasy", "Children"]),

            new("Harry Potter and the Deathly Hallows", "J.K. Rowling", "Bloomsbury", "9780747591054",
                "Harry, Ron and Hermione set out on a mission to destroy the last of Voldemort's Horcruxes.",
                607, 2007, CoverType.Hardcover, Language.English, 4, ["Fantasy", "Children"]),
 
            // George R.R. Martin
            new("A Game of Thrones", "George R.R. Martin", "Tor Books", "9780553103540",
                "Seven noble families fight for control of the Iron Throne of the Seven Kingdoms of Westeros.",
                694, 1996, CoverType.Hardcover, Language.English, 3, ["Fantasy", "Historical Novels"]),

            new("A Clash of Kings", "George R.R. Martin", "Tor Books", "9780553108033",
                "The War of the Five Kings erupts as multiple claimants fight for the Iron Throne.",
                761, 1998, CoverType.Hardcover, Language.English, 3, ["Fantasy", "Historical Novels"]),

            new("A Storm of Swords", "George R.R. Martin", "Tor Books", "9780553106633",
                "The Red Wedding shocks the realm as the war grinds on with devastating consequences.",
                973, 2000, CoverType.Hardcover, Language.English, 3, ["Fantasy", "Historical Novels"]),
 
            // Stephen King
            new("It", "Stephen King", "Hodder & Stoughton", "9780340392553",
                "A shapeshifting creature that preys on children resurfaces in the town of Derry, Maine every 27 years.",
                1138, 1986, CoverType.Hardcover, Language.English, 3, ["Horror"]),

            new("The Shining", "Stephen King", "Doubleday", "9780385121675",
                "A family heads to an isolated hotel for the winter where a sinister presence influences the alcoholic father.",
                447, 1977, CoverType.Softcover, Language.English, 4, ["Horror"]),

            new("Misery", "Stephen King", "Hodder & Stoughton", "9780450417399",
                "A famous novelist is rescued from a car crash by his self-proclaimed number one fan who holds him captive.",
                310, 1987, CoverType.Softcover, Language.English, 4, ["Horror", "Thrillers and Crimes"]),

            new("The Stand", "Stephen King", "Doubleday", "9780385121682",
                "A plague kills most of humanity and survivors gather in two camps for an apocalyptic battle between good and evil.",
                1153, 1978, CoverType.Hardcover, Language.English, 3, ["Horror", "Fantasy"]),

            new("Pet Sematary", "Stephen King", "Doubleday", "9780385182713",
                "A family discovers a burial ground near their new home that has the power to bring the dead back to life.",
                374, 1983, CoverType.Softcover, Language.English, 4, ["Horror"]),
 
            // Agatha Christie
            new("Murder on the Orient Express", "Agatha Christie", "HarperCollins", "9780007119318",
                "Hercule Poirot investigates the murder of a passenger aboard the famous Orient Express train.",
                256, 1934, CoverType.Softcover, Language.English, 5, ["Thrillers and Crimes", "World Classics"]),

            new("And Then There Were None", "Agatha Christie", "HarperCollins", "9780007136834",
                "Ten strangers are lured to an isolated island and murdered one by one according to a nursery rhyme.",
                272, 1939, CoverType.Softcover, Language.English, 5, ["Thrillers and Crimes", "World Classics"]),

            new("Death on the Nile", "Agatha Christie", "HarperCollins", "9780007119325",
                "Hercule Poirot must solve the murder of a young heiress during a cruise on the Nile.",
                288, 1937, CoverType.Softcover, Language.English, 4, ["Thrillers and Crimes"]),
 
            // Ernest Hemingway
            new("The Old Man and the Sea", "Ernest Hemingway", "Simon & Schuster", "9780684801223",
                "An aging Cuban fisherman struggles with a giant marlin far out in the Gulf Stream over several days.",
                127, 1952, CoverType.Softcover, Language.English, 5, ["World Classics"]),

            new("A Farewell to Arms", "Ernest Hemingway", "Simon & Schuster", "9780684801469",
                "An American ambulance officer falls in love with a British nurse during the chaos of World War I.",
                332, 1929, CoverType.Softcover, Language.English, 4, ["World Classics", "Romance Novels"]),
 
            // Gabriel García Márquez
            new("One Hundred Years of Solitude", "Gabriel García Márquez", "HarperCollins", "9780060883287",
                "The multi-generational story of the Buendía family in the magical fictional town of Macondo.",
                417, 1967, CoverType.Hardcover, Language.English, 4, ["World Classics", "Contemporary Prose"]),

            new("Love in the Time of Cholera", "Gabriel García Márquez", "Penguin Random House", "9780307389732",
                "A tale of unrequited love and obsession that spans over fifty years in a Colombian city.",
                348, 1985, CoverType.Softcover, Language.English, 3, ["World Classics", "Romance Novels"]),
 
            // Leo Tolstoy
            new("War and Peace", "Leo Tolstoy", "Penguin Random House", "9780143039990",
                "A sweeping epic of Russian society and the Napoleonic Wars told through five aristocratic families.",
                1225, 1869, CoverType.Hardcover, Language.English, 3, ["World Classics", "Historical Novels"]),

            new("Anna Karenina", "Leo Tolstoy", "Penguin Random House", "9780143035008",
                "A tragic story about the doomed love affair of the married aristocrat Anna Karenina.",
                864, 1877, CoverType.Hardcover, Language.English, 3, ["World Classics", "Romance Novels"]),
 
            // Fyodor Dostoevsky
            new("Crime and Punishment", "Fyodor Dostoevsky", "Penguin Random House", "9780143058144",
                "A young student murders a pawnbroker and grapples with guilt, morality and redemption.",
                551, 1866, CoverType.Softcover, Language.English, 4, ["World Classics"]),

            new("The Brothers Karamazov", "Fyodor Dostoevsky", "Penguin Random House", "9780374528379",
                "A philosophical novel exploring faith, doubt, free will and morality through three brothers.",
                796, 1880, CoverType.Hardcover, Language.English, 3, ["World Classics"]),

            new("The Idiot", "Fyodor Dostoevsky", "Penguin Random House", "9780142437957",
                "A kind and guileless man returns to Russia from a Swiss sanatorium and is unable to cope with society.",
                668, 1869, CoverType.Softcover, Language.English, 3, ["World Classics"]),
 
            // George Orwell
            new("1984", "George Orwell", "Penguin Random House", "9780451524935",
                "Winston Smith struggles against the totalitarian Party and its leader Big Brother in a dystopian future.",
                328, 1949, CoverType.Softcover, Language.English, 7, ["World Classics", "Fantasy"]),

            new("Animal Farm", "George Orwell", "Penguin Random House", "9780451526342",
                "Farm animals overthrow their human farmer only to find a new tyranny emerging among themselves.",
                112, 1945, CoverType.Softcover, Language.English, 6, ["World Classics"]),
 
            // Aldous Huxley
            new("Brave New World", "Aldous Huxley", "HarperCollins", "9780060850524",
                "A futuristic society where humans are engineered in hatcheries and controlled through conditioning.",
                311, 1932, CoverType.Softcover, Language.English, 5, ["World Classics", "Fantasy"]),
 
            // Ray Bradbury
            new("Fahrenheit 451", "Ray Bradbury", "Simon & Schuster", "9781451673319",
                "In a future society where books are burned, a fireman begins secretly reading and questioning everything.",
                158, 1953, CoverType.Softcover, Language.English, 5, ["Fantasy"]),
 
            // Isaac Asimov
            new("Foundation", "Isaac Asimov", "Tor Books", "9780553293357",
                "Mathematician Hari Seldon devises a plan to preserve civilization as the Galactic Empire crumbles.",
                244, 1951, CoverType.Softcover, Language.English, 4, ["Fantasy"]),

            new("I, Robot", "Isaac Asimov", "Tor Books", "9780553294385",
                "Nine linked short stories exploring the complex and sometimes dangerous interaction between humans and robots.",
                224, 1950, CoverType.Softcover, Language.English, 4, ["Fantasy", "Science and Technology"]),
 
            // Arthur Conan Doyle
            new("The Hound of the Baskervilles", "Arthur Conan Doyle", "Penguin Random House", "9780141034959",
                "Sherlock Holmes investigates a spectral hound that has supposedly killed members of the Baskerville family.",
                256, 1902, CoverType.Softcover, Language.English, 5, ["Thrillers and Crimes", "World Classics"]),

            new("A Study in Scarlet", "Arthur Conan Doyle", "Penguin Random House", "9780140439083",
                "The first appearance of Sherlock Holmes and Dr. Watson as they investigate a mysterious murder in London.",
                192, 1887, CoverType.Softcover, Language.English, 4, ["Thrillers and Crimes", "World Classics"]),
 
            // Dan Brown
            new("The Da Vinci Code", "Dan Brown", "Doubleday", "9780385504201",
                "Harvard symbologist Robert Langdon uncovers a conspiracy hidden in the works of Leonardo Da Vinci.",
                689, 2003, CoverType.Softcover, Language.English, 5, ["Thrillers and Crimes"]),

            new("Angels and Demons", "Dan Brown", "Simon & Schuster", "9780671027360",
                "Robert Langdon races against time to stop the Illuminati from destroying the Vatican.",
                616, 2000, CoverType.Softcover, Language.English, 4, ["Thrillers and Crimes"]),
 
            // Paulo Coelho
            new("The Alchemist", "Paulo Coelho", "HarperCollins", "9780062315007",
                "A shepherd boy travels from Spain to Egypt following his dream of finding a worldly treasure.",
                163, 1988, CoverType.Softcover, Language.English, 7, ["Contemporary Prose"]),
 
            // Haruki Murakami
            new("Norwegian Wood", "Haruki Murakami", "Penguin Random House", "9780375704024",
                "A nostalgic story of loss and coming of age set in the student upheaval of late 1960s Tokyo.",
                296, 1987, CoverType.Softcover, Language.English, 4, ["Contemporary Prose", "Romance Novels"]),

            new("Kafka on the Shore", "Haruki Murakami", "Penguin Random House", "9781400079278",
                "A teenage runaway and an aging man with a strange gift embark on intertwined mystical journeys.",
                467, 2002, CoverType.Softcover, Language.English, 3, ["Contemporary Prose", "Fantasy"]),
 
            // Frank Herbert
            new("Dune", "Frank Herbert", "Tor Books", "9780441013593",
                "On the desert planet Arrakis, Paul Atreides is thrust into a battle for control of the universe's most valuable resource.",
                688, 1965, CoverType.Hardcover, Language.English, 4, ["Fantasy"]),
 
            // Douglas Adams
            new("The Hitchhiker's Guide to the Galaxy", "Douglas Adams", "Macmillan Publishers", "9780330258647",
                "Moments before Earth is demolished, Arthur Dent is whisked into a mad adventure across the universe.",
                193, 1979, CoverType.Softcover, Language.English, 5, ["Fantasy", "Humor"]),
 
            // Bulgarian authors
            new("Под игото", "Иван Вазов", "Ciela", "9789542806271",
                "Роман за живота на българите преди и по време на Априлското въстание от 1876 г.",
                512, 1894, CoverType.Softcover, Language.Bulgarian, 6, ["Bulgarian Prose", "Historical Novels"]),

            new("Бай Ганьо", "Алеко Константинов", "Colibri", "9789540701721",
                "Сатирични разкази за Бай Ганьо Балкански и неговото пътуване из Европа.",
                256, 1895, CoverType.Softcover, Language.Bulgarian, 5, ["Bulgarian Prose", "Humor"]),

            new("Старопланински легенди", "Йордан Йовков", "Janet 45", "9789546074881",
                "Сборник разкази за живота, любовта и честта на хората в Стара планина.",
                184, 1927, CoverType.Softcover, Language.Bulgarian, 5, ["Bulgarian Prose"]),

            new("Под манастирската лоза", "Елин Пелин", "Hermes", "9789542602866",
                "Хумористични разкази за живота на монасите в един български манастир.",
                198, 1936, CoverType.Softcover, Language.Bulgarian, 4, ["Bulgarian Prose", "Humor"]),
        ];
    }
}
