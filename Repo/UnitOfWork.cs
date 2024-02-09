using PractiseTest1.Entities;
namespace PractiseTest1.Repo
{
    public class UnitOfWork
    {
        BookDbContext context = null;
        BookImpl bookImpl = null;
        UserImpl userImpl = null;


        public UnitOfWork(BookDbContext ctx)
        {
            context = ctx;
        }

        public BookImpl BookImplObject
        {
            get
            {
                if (bookImpl == null)
                    bookImpl = new BookImpl(context);
                return bookImpl;
            }
        }

        public UserImpl UserImplObject
        {
            get
            {
                if (userImpl == null)
                    userImpl = new UserImpl(context);
                return userImpl;

            }
        }

        public void SaveAll()
        {
            if (context != null)
            {
                context.SaveChanges();
            }
        }
    }
}
