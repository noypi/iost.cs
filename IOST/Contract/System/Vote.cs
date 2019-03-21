// generated by IOST.DevTools last: 3/21/2019 12:22:56 PM
namespace IOST.Contract.System
{
    /// <summary>
    /// A universal voting contract used to create votes, collect votes, and vote on statistics. You can implement your own voting function based on this contract.
    /// 
    /// Version: javascript
    /// Language: 1.0.0
    /// Reference: https://developers.iost.io/docs/en/6-reference/SystemContract.html
    /// </summary>
    public partial class Vote : Contract
    {
        private const string Cid = "vote.iost";

        /// <summary>
        /// Create a vote.
        /// </summary>
        /// <param name="voteCreatorAccountName">Create a vote that requires pledge 1000 IOST, which will be deducted from the creator account, and the creator account has the admin privilege to vote</param>
        /// <param name="voteDescription"></param>
        /// <param name="votingSettings">contains 5 keys: resultNumber —— number type, number of voting results, maximum 2000;  minVote —— number type, minimum number of votes, candidates with more votes than this number In order to enter the voting result set; options - array type, candidate set, each item is a string, represents a candidate, the initial can be empty []; anyOption - bool type, whether to allow The candidate in the non-options collection, passing false means that the user can only cast candidates in the options collection; freezeTime - number type, cancel the token freeze time, in seconds;</param>
        public static void NewVote(Transaction tx, string voteCreatorAccountName, string voteDescription, object votingSettings)
        {
            tx.AddAction(Cid, "newVote", voteCreatorAccountName, voteDescription, votingSettings);
        }

        /// <summary>
        /// Increase voting options.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        /// <param name="options"></param>
        /// <param name="whetherToClearThePreviousVotes"></param>
        public static void AddOption(Transaction tx, string voteID, string options, bool whetherToClearThePreviousVotes)
        {
            tx.AddAction(Cid, "addOption", voteID, options, whetherToClearThePreviousVotes);
        }

        /// <summary>
        /// Delete the voting option, but retain the result of the vote, delete it, and then add this option through AddOption to choose whether to restore the number of votes.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        /// <param name="options"></param>
        /// <param name="whetherToForceDelete">false means that the option is not deleted when it is in the result set, true means to force delete and update the result set</param>
        public static void RemoveOption(Transaction tx, string voteID, string options, bool whetherToForceDelete)
        {
            tx.AddAction(Cid, "removeOption", voteID, options, whetherToForceDelete);
        }

        /// <summary>
        /// Get the votes for the candidate.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        /// <param name="options"></param>
        public static void GetOption(Transaction tx, string voteID, string options)
        {
            tx.AddAction(Cid, "getOption", voteID, options);
        }

        /// <summary>
        /// Vote on behalf of others, the IOST of the voting pledge will be deducted from the agent account.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        /// <param name="agentAccountName"></param>
        /// <param name="voterAccountName"></param>
        /// <param name="options"></param>
        /// <param name="numberOfVotes"></param>
        public static void VoteFor(Transaction tx, string voteID, string agentAccountName, string voterAccountName, string options, string numberOfVotes)
        {
            tx.AddAction(Cid, "voteFor", voteID, agentAccountName, voterAccountName, options, numberOfVotes);
        }

        /// <summary>
        /// vote.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        /// <param name="voterAccountName"></param>
        /// <param name="options"></param>
        /// <param name="numberOfVotes"></param>
        public static void Vote_(Transaction tx, string voteID, string voterAccountName, string options, string numberOfVotes)
        {
            tx.AddAction(Cid, "vote", voteID, voterAccountName, options, numberOfVotes);
        }

        /// <summary>
        /// Cancel the vote.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        /// <param name="voterAccountName"></param>
        /// <param name="options"></param>
        /// <param name="numberOfVotes"></param>
        public static void Unvote(Transaction tx, string voteID, string voterAccountName, string options, string numberOfVotes)
        {
            tx.AddAction(Cid, "unvote", voteID, voterAccountName, options, numberOfVotes);
        }

        /// <summary>
        /// Get an account vote record.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        /// <param name="voterAccountName"></param>
        public static void GetVote(Transaction tx, string voteID, string voterAccountName)
        {
            tx.AddAction(Cid, "getVote", voteID, voterAccountName);
        }

        /// <summary>
        /// Get the voting result and return the option of resultNumber before the number of votes.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        public static void GetResult(Transaction tx, string voteID)
        {
            tx.AddAction(Cid, "getResult", voteID);
        }

        /// <summary>
        /// Delete the vote and return the IOST that was created during the voting to the creator account.
        /// </summary>
        /// <param name="voteID">ID returned by the NewVote interface</param>
        public static void DelVote(Transaction tx, string voteID)
        {
            tx.AddAction(Cid, "delVote", voteID);
        }
    }
}
