
using MongoDBLabb;


IStringIO io;
IEntryDAO entryDAO;

io = new TextIO();
entryDAO = new MongoDAO(ConnectionString.connectionStr, "Diary");

DiaryController controller = new DiaryController(io, entryDAO);

controller.Start();