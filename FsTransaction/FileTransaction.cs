using System.Transactions;

namespace FsTransaction
{
    public class FileTransaction : IEnlistmentNotification
    {
        public void Commit(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        public void Rollback(Enlistment enlistment)
        {
            enlistment.Done();
        }
    }
}
