using System;
using System.Collections.Generic;
using System.Text;

namespace IOST.Contract.System
{
    /// <summary>
    /// https://developers.iost.io/docs/en/6-reference/SystemContract.html#authiost
    /// </summary>
    public class Auth : Contract
    {
        const string CID = "auth.iost";

        public static void SignUp(Transaction tx, string name, string ownerkey, string activekey)
        {
            tx.AddAction(CID, "signUp", name, ownerkey, activekey);
        }

        public static void AddPermission(Transaction tx, string username, string permissionName, double permissionThreshold)
        {
            tx.AddAction(CID, "addPermission", username, permissionName, permissionThreshold);
        }

        public static void DropPermission(Transaction tx, string username, string permissionName)
        {
            tx.AddAction(CID, "dropPermission", username, permissionName);
        }

        public static void AssignPermission(Transaction tx, string username, string permissions, string item, double weight)
        {
            tx.AddAction(CID, "assignPermission", username, permissions, item, weight);
        }

        public static void AddGroup(Transaction tx, string username, string groupName)
        {
            tx.AddAction(CID, "addGroup", username, groupName);
        }

        public static void DropGroup(Transaction tx, string username, string groupName)
        {
            tx.AddAction(CID, "dropGroup", username, groupName);
        }

        public static void AssignGroup(Transaction tx, string username, string groupName, string item, double weight)
        {
            tx.AddAction(CID, "assignGroup", username, groupName, item, weight);
        }

        public static void RevokeGroup(Transaction tx, string username, string groupName, string item)
        {
            tx.AddAction(CID, "revokeGroup", username, groupName, item);
        }

        public static void AssignPermissionToGroup(Transaction tx, string username, string permissionName, string groupName)
        {
            tx.AddAction(CID, "assignPermissionToGroup", username, permissionName, groupName);
        }

        public static void RevokePermissionInGroup(Transaction tx, string username, string permissionName, string groupName)
        {
            tx.AddAction(CID, "revokePermissionInGroup", username, permissionName, groupName);
        }
    }
}
