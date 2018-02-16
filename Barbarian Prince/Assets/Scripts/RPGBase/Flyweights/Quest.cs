using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGBase.Flyweights
{
    public sealed class Quest
    {
        /// <summary>
        /// the quest id.
        /// </summary>
        public string Ident { get; }
        /// <summary>
        /// the root.
        /// </summary>
        private QuestBranch root;
        private QuestBranch Root
        {
            get { return root; }
            set
            {
                root = value;
                try
                {
                    root.Taken = true;
                }
                catch (RPGException e)
                {
                    // we'll NEEEVER get here
                }
            }
        }
        /// <summary>
        /// the quest title.
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// Creates a new instance of <see cref="Quest"/>.
        /// </summary>
        /// <param name="id">the quest id</param>
        /// <param name="t">the quest title</param>
        public Quest(String id, String t)
        {
            Ident = id;
            Title = t;
        }
        /// <summary>
        /// Finds a branch with a specific id.
        /// </summary>
        /// <param name="id">the branch's id</param>
        /// <returns><see cref="QuestBranch"/> or <see cref="null"/> if none was found</returns>
        public QuestBranch GetBranch(string id)
        {
            // loop through all branches until id is found.
            QuestBranch found = null;
            if (string.Equals(root.RefId, id, StringComparison.OrdinalIgnoreCase))
            {
                found = root;
            }
            else
            {
                found = root.FindChild(id);
            }
            return found;
        }
        public override string ToString()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            try
            {
                QuestBranch branch = root;
                sb.Append(branch.Localized);
                sb.Append('\n');
                while (branch.Branches.Length > 0)
                {
                    int i = branch.Branches.Length - 1;
                    for (; i >= 0; i--)
                    {
                        // go through all branches
                        if (branch.Branches[i].Taken)
                        {
                            // follow branch that was taken
                            branch = branch.Branches[i];
                            sb.Append('\n');
                            sb.Append(branch.Localized);
                            sb.Append('\n');
                            break;
                        }
                    }
                    if (i == -1)
                    {
                        // made it through all branches
                        break;
                    }
                }
            }
            catch (PooledException e)
            {
                // e.printStackTrace();
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
    }
    public sealed class QuestBranch
    {
        private QuestBranch[] branches;
        /// <summary>
        /// the list of branches this branch leads to.
        /// </summary>
        public QuestBranch[] Branches { get { return branches; } }
        /// <summary>
        /// the localized quest text.
        /// </summary>
        public string Localized { get; }
        /// <summary>
        /// the branch's reference id.
        /// </summary>
        public string RefId { get; }
        /// <summary>
        /// the branch's root.
        /// </summary>
        private QuestBranch Root { get; set; }
        private bool taken;
        /// <summary>
        /// flag indicating the branch has been taken.
        /// </summary>
        public bool Taken
        {
            get { return taken; }
            set
            {
                if (Root != null)
                {
                    if (!Root.Taken)
                    {
                        throw new RPGException(ErrorMessage.ILLEGAL_OPERATION, "Cannot take a child branch before its parent.");
                    }
                    for (int i = Root.Branches.Length - 1; i >= 0; i--)
                    {
                        if (Root.Branches[i].Taken)
                        {
                            throw new RPGException(ErrorMessage.ILLEGAL_OPERATION, "A branch on this level was already taken.");
                        }
                    }
                }
                taken = value;
            }
        }
        /// <summary>
        /// the XP reward given when the branch is taken.
        /// </summary>
        public long XpReward { get; }
        /// <summary>
        /// Creates a new instance of <see cref="QuestBranch"/>.
        /// </summary>
        /// <param name="id">the branch's reference id</param>
        /// <param name="text">the localized quest text</param>
        /// <param name="xp">the XP reward given when the branch is taken</param>
        QuestBranch(string id, string text, long xp)
        {
            RefId = id;
            Localized = text;
            XpReward = xp;
            branches = new QuestBranch[0];
        }
        /// <summary>
        /// Adds a branch this <see cref="QuestBranch"/> leads to.
        /// </summary>
        /// <param name="branch">the new <see cref="QuestBranch"/></param>
        public void AddBranch(QuestBranch branch)
        {
            branches = ArrayUtilities.Instance.ExtendArray(branch, branches);
            branch.Root = this;
        }
        /// <summary>
        /// Finds a child branch with a specific id.
        /// </summary>
        /// <param name="id">the branch's id</param>
        /// <returns><see cref="QuestBranch"/> or <see cref="null"/> if none was found</returns>
        public QuestBranch FindChild(string id)
        {
            QuestBranch child = null;
            for (int i = branches.Length - 1; i >= 0; i--)
            {
                if (string.Equals(branches[i].RefId, id, StringComparison.OrdinalIgnoreCase))
                {
                    child = branches[i];
                    break;
                }
                child = branches[i].FindChild(id);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
