using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMSApi.Api.Response;

namespace smsapiTests.Contacts
{
    [TestClass]
    public class GroupsTest : ContactsTestBase
    {
        private Group _group;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();

            _group = _factory.CreateGroup().SetName("exampleGroup").Execute();
        }

        [TestCleanup]
        public void Cleanup()
        {
            var groups = _factory.ListGroups().Execute();
            foreach (var group in groups.Collection)
            {
                _factory.DeleteGroup(group.Id).Execute();
            }
        }

        [TestMethod]
        public void CreateGroup()
        {
            var group = _factory.CreateGroup().SetName("exampleGroup1").Execute();

            Assert.AreEqual("exampleGroup1", group.Name);
            Assert.IsNotNull(group.Id);
        }

        [TestMethod]
        public void EditGroup()
        {
            var response = _factory.EditGroup(_group.Id).SetName("GroupY").Execute();

            Assert.AreEqual(_group.Id, response.Id);
            Assert.AreNotEqual(_group.Name, response.Name);
            Assert.AreEqual("GroupY", response.Name);
            Assert.AreEqual(_group.Idx, response.Idx);
            Assert.AreEqual(_group.Description, response.Description);
        }

        [TestMethod]
        public void GetGroup()
        {
            var response = _factory.GetGroup(_group.Id).Execute();

            Assert.AreEqual(_group.Id, response.Id);
            Assert.AreEqual(_group.Name, response.Name);
            Assert.AreEqual(_group.Idx, response.Idx);
            Assert.AreEqual(_group.Description, response.Description);
        }

        [TestMethod]
        public void ListGroups()
        {
            _factory.ListGroups().Execute();
        }

        [TestMethod]
        public void ListGroupsWithFilterByName()
        {
            var listResponse = _factory.ListGroups().SetName("exampleGroup").Execute();

            Assert.AreEqual(1, listResponse.Collection.Count);
        }

        [TestMethod]
        public void ListWithFilterByName_ShouldNotFound()
        {
            var listResponse = _factory.ListGroups().SetName("missingGroup").Execute();

            Assert.AreEqual(0, listResponse.Collection.Count);
        }

        [TestMethod]
        public void CreateGroupPermission()
        {
            var groupPermission = _factory.CreateGroupPermission(_group.Id)
                                .SetUsername(_username)
                                .SetRead(true)
                                .SetWrite(false)
                                .SetSend(false)
                                .Execute();

            Assert.AreEqual(_username, groupPermission.Username);
        }

        [TestMethod]
        public void EditGroupPermission()
        {
            var groupPermission = _factory.EditGroupPermission(_group.Id, _username)
                                .SetRead(true)
                                .SetWrite(true)
                                .SetSend(true)
                                .Execute();

            Assert.IsTrue(groupPermission.Read);
            Assert.IsTrue(groupPermission.Write);
            Assert.IsTrue(groupPermission.Send);
        }

        [TestMethod]
        public void ListGroupPermissions()
        {
            _factory.ListGroupPermissions(_group.Id).Execute();
        }
    }
}
