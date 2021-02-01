using clsItemLanguages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI
{
    /// <summary>
    /// This Class is a singleton class
    /// </summary>
    public class ItemMasterManager
    {
        private static ItemMasterManager instance = null;

        #region lstConnectionString
        
        private string ConnectionString = @"Data Source=.;Initial Catalog=ItemLanguage;Persist Security Info=True;User ID=sa;Password=@dm1n";

        #endregion

        protected List<Item> lstItems = new List<Item>();                           // Key is ItemId
        //protected Dictionary<string, ItemComponent> lstItems = new Dictionary<string, ItemComponent>();                           // Key is ItemId
        protected Dictionary<string, ItemType> lstItemTypes = new Dictionary<string, ItemType>();               // Key is ItemTypeId
        protected Dictionary<string, Language> lstLanguages = new Dictionary<string, Language>();               // Key is LanguageId
        protected Dictionary<string, ItemData> lstItemData = new Dictionary<string, ItemData>();                // Key is ItemDataId
        protected List<RelatedItems> lstRelatedItems = new List<RelatedItems>();

        protected Dictionary<string, Group> lstGroup = new Dictionary<string, Group>();
        protected Dictionary<string, GroupItem> lstGroupItem = new Dictionary<string, GroupItem>();
        protected Dictionary<string, GroupTreeNode> lstGroupTreeNode = new Dictionary<string, GroupTreeNode>();

        protected Dictionary<string, Tag> lstTag = new Dictionary<string, Tag>();
        protected Dictionary<string, ItemTag> lstItemTag = new Dictionary<string, ItemTag>();
        protected Dictionary<string, TagType> lstTagType = new Dictionary<string, TagType>();
        protected Dictionary<string, TagTypeConstraint> lstTagTypeCon = new Dictionary<string, TagTypeConstraint>();
        protected Dictionary<string, ItemTagHistory> lstItemTagHis = new Dictionary<string, ItemTagHistory>();

        protected Dictionary<string, ItemFunction> lstItemFunctions = new Dictionary<string, ItemFunction>();
        protected Dictionary<string, GoingType> lstGoingType = new Dictionary<string, GoingType>();
        protected Dictionary<string, ItemGoing> lstItemGoing = new Dictionary<string, ItemGoing>();
        protected Dictionary<string, GroupGoing> lstGroupGoing = new Dictionary<string, GroupGoing>();
        protected Dictionary<string, ObjectGoingWith> lstObjGoingWith = new Dictionary<string, ObjectGoingWith>();
        protected Dictionary<string, GoingObject> lstGoingObj = new Dictionary<string, GoingObject>();
        protected Dictionary<string, ItemGoingWith> lstItemGoingWith = new Dictionary<string, ItemGoingWith>();


        protected Dictionary<string, Transaction> lstTransaction = new Dictionary<string, Transaction>();
        protected Dictionary<string, DIYDefinition> lstDIYDef = new Dictionary<string, DIYDefinition>();
        protected Dictionary<string, DIYFixedPrice> lstDIYFixedPrice = new Dictionary<string, DIYFixedPrice>();
        protected Dictionary<string, DIYPrice> lstDIYPrice = new Dictionary<string, DIYPrice>();
        protected Dictionary<string, DIYPriceType> lstDIYPriceType = new Dictionary<string, DIYPriceType>();
        protected Dictionary<string, DIYProfile> lstDIYProfile = new Dictionary<string, DIYProfile>();
        protected Dictionary<string, DIYSelection> lstDIYSelection = new Dictionary<string, DIYSelection>();
        protected Dictionary<string, DIYSelectionType> lstDIYSelectType = new Dictionary<string, DIYSelectionType>();
        protected Dictionary<string, Period> lstPeriod = new Dictionary<string, Period>();
        protected Dictionary<string, StandardItemPrice> lstStdItmPrice = new Dictionary<string, StandardItemPrice>();
        protected Dictionary<string, StandardItemProfile> lstStdItmProfile = new Dictionary<string, StandardItemProfile>(); 
        protected Dictionary<string, Status> lstStatus = new Dictionary<string, Status>(); 



        private ItemMasterManager() { }

        public static ItemMasterManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ItemMasterManager();
            }
            return instance;
        }

        public ItemType GetItemTypeById(string itemTypeId)
        {
            try
            {
                return lstItemTypes[itemTypeId];
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ItemType> GetAllItemType()
        {
            return lstItemTypes.Values.ToList();
        }

        public Item GetItemById(string itemId)
        {
            try
            {
                return lstItems.First(r => r.Id == itemId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Item> GetItemByItemType(string itemTypeId)
        {
            return lstItems.Where(r => r.ItemType.Id == itemTypeId).ToList();
        }

        public double GetStdPriceByItemId(string itemId)
        {
            try
            {
                var getLstStdPrice = GetStdItmPriceByItemId(itemId);
                return getLstStdPrice.First(r => r.Period.Status.StatusName == "Active").StandardPrice;
            }
            catch(Exception ex)
            {
                return 0;
            }
            
        }

        public List<Item> GetItemExceptItemTypeId(string itemTypeId)
        {
            return lstItems.Where(r => r.ItemType.Id != itemTypeId).ToList();
        }

        public List<Item> GetAllItem()
        {
            return lstItems.ToList();
        }
        public List<RelatedItems> GetAllMappingItems()
        {
            return lstRelatedItems;
        }
        public List<RelatedItems> GetRelatedItemsByParentId(string parentId)
        {
            return lstRelatedItems.Where(r => r.ParentItem.Id == parentId).ToList();
        }
        public Language GetLanguageById(string langId)
        {
            return lstLanguages[langId];
        }
        public List<Language> GetAllLanguage()
        {
            return lstLanguages.Values.ToList();
        }

        public List<ItemData> GetAllItemData()
        {
            return lstItemData.Values.ToList();
        }

        public IEnumerable<ItemData> GetItemDataByItem(string itemId)
        {
            return lstItemData.Values.Where(r => r.Item.Id == itemId);
        }
        public IEnumerable<ItemData> GetItemDataByLang(string langId)
        {
            return lstItemData.Values.Where(r => r.Language.Id == langId);
        }

        public List<Group> GetAllGroup()
        {
            return lstGroup.Values.ToList();
        }
        public Group GetGroupById(string groupId)
        {
            return lstGroup[groupId];
        }

        public List<GroupItem> GetAllGroupItem()
        {
            return lstGroupItem.Values.ToList();
        }
        public List<GroupItem> GetGroupItemByItem(Item item)
        {
            return item.GetGroupItem(this.lstGroupItem.Values.ToList());
        }
        public List<GroupItem> GetGroupItemByGroup(Group group)
        {
            try
            {
                return group.GetGroupItem(lstGroupItem.Values.ToList());
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public List<GroupTreeNode> GetAllGroupTreeNode()
        {
            return lstGroupTreeNode.Values.ToList();
        }
        public GroupTreeNode GetGroupTreeNodeByChildGroupId(string childGroupId)
        {
            return lstGroupTreeNode.Values.First(r => r.Group.Id == childGroupId);
        }
        public List<GroupTreeNode> GetGroupTreeNodeByParentId(string parentGroupId)
        {
            List<GroupTreeNode> lstTreeNode = new List<GroupTreeNode>();
            foreach (var tmp in lstGroupTreeNode.Values)
            {
                if (tmp.ParentGroup != null)
                {
                    if (tmp.ParentGroup.Id == parentGroupId)
                    {
                        lstTreeNode.Add(tmp);
                    }
                }
            }
            return lstTreeNode;
        }

        public List<Tag> GetAllTag()
        {
            return lstTag.Values.ToList();
        }
        public Tag GetTagById(string tagId)
        {
            return lstTag[tagId];
        }

        public Dictionary<string, Tag> GetTagByTagType(string tagTypeId)
        {
            Dictionary<string, Tag> lstTags = new Dictionary<string, Tag>();
            foreach (var temp in lstTag)
            {
                if (temp.Value.TagType.Id == tagTypeId)
                {
                    lstTag.Add(tagTypeId, temp.Value);
                }
            }

            return lstTag;
        }
        public Dictionary<string, Tag> GetTagByTagTypeCon(string tagTypeConId)
        {
            Dictionary<string, Tag> lstTags = new Dictionary<string, Tag>();
            foreach (var temp in lstTag)
            {
                if (temp.Value.TagTypeCon.Id == tagTypeConId)
                {
                    lstTag.Add(tagTypeConId, temp.Value);
                }
            }

            return lstTag;
        }
        public Tag GetTagByTagType_And_Con(string tagTypeId, string tagTypeConId)
        {
            foreach (var temp in lstTag)
            {
                if (temp.Value.TagTypeCon.Id == tagTypeConId && temp.Value.TagType.Id == tagTypeId)
                {
                    return temp.Value;
                }
            }

            return null;
        }

        public List<TagType> GetAllTagType()
        {
            return lstTagType.Values.ToList();
        }

        public TagType GetTagTypeById(string tagTypeId)
        {
            return lstTagType[tagTypeId];
        }
        public List<TagTypeConstraint> GetAllTagTypeCon()
        {
            return lstTagTypeCon.Values.ToList();
        }
        public TagTypeConstraint GetTagTypeConById(string tagTypeConId)
        {
            return lstTagTypeCon[tagTypeConId];
        }
        public TagTypeConstraint GetTagTypeConByName(string tagConName)
        {
            foreach (var tmp in lstTagTypeCon)
            {
                if (tmp.Value.Name == tagConName)
                {
                    return tmp.Value;
                }
            }

            return null;
        }
        public ItemTag GetItemTagById(string itemTagId)
        {
            return lstItemTag[itemTagId];
        }
        public List<ItemTag> GetAllItemTag()
        {
            return lstItemTag.Values.ToList();
        }
        public List<ItemTag> GetItemTagByTag(Tag tag)
        {
            List<ItemTag> tempList = new List<ItemTag>();

            foreach (var temp in lstItemTag)
            {
                if (temp.Value.Tag.Id == tag.Id)
                {
                    tempList.Add(temp.Value);
                }
            }

            return tempList;
        }

        public List<ItemTag> GetItemTagByItem(Item item)
        {
            List<ItemTag> tempList = new List<ItemTag>();

            foreach (var temp in lstItemTag)
            {
                if (temp.Value.Item.Id == item.Id)
                {
                    tempList.Add(temp.Value);
                }
            }

            return tempList;
        }

        public ItemTagHistory GetItemTagHistoryByItemTag(string itemTag)
        {

            foreach (var temp in lstItemTagHis)
            {
                if (temp.Key == itemTag)
                {
                    return temp.Value;
                }
            }
            return null;

        }
        public List<ItemTagHistory> GetAllItemTagHistory()
        {
            return lstItemTagHis.Values.ToList();
        }

        public List<ItemTagHistory> GetItemTagHistoryByItem(string itemId)
        {
            List<ItemTagHistory> lstHistory = new List<ItemTagHistory>();
            foreach (var tmp in lstItemTagHis)
            {
                if (tmp.Value.Item.Id == itemId)
                {
                    lstHistory.Add(tmp.Value);
                }
            }
            return lstHistory;
        }


        public ItemFunction GetItemFunctionById(string id)
        {
            return lstItemFunctions[id];
        }
        public Dictionary<string, ItemFunction> GetAllItemFunction()
        {
            return lstItemFunctions;
        }
        public GoingType GetGoingTypeById(string id)
        {
            try
            {
                return lstGoingType[id];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Dictionary<string, GoingType> GetAllGoingType()
        {
            return lstGoingType;
        }

        public List<GoingType> GetGoingTypeByItemFunc(string itemFuncId)
        {
            List<GoingType> lstTemp = new List<GoingType>();
            foreach (var tmp in lstGoingType)
            {
                if (tmp.Value.itmFunc.Id == itemFuncId)
                {
                    lstTemp.Add(tmp.Value);
                }
            }
            return lstTemp;
        }
        public ItemGoing GetItemGoingById(string id)
        {
            try
            {
                return lstItemGoing[id];
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Dictionary<string, ItemGoing> GetAllItemGoing()
        {
            return lstItemGoing;
        }
        public GroupGoing GetGroupGoingById(string id)
        {
            try
            {
                return lstGroupGoing[id];
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public Dictionary<string, GroupGoing> GetAllGroupGoing()
        {
            return lstGroupGoing;
        }
        public GoingObject GetGoingObjectById(string id)
        {
            try
            {
                return lstGoingObj[id];
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        public Dictionary<string, GoingObject> GetAllGoingObject()
        {
            return lstGoingObj;
        }
        public Dictionary<string, GoingObject> GetGoingObjectByGoingType(string goingTypeId)
        {
            Dictionary<string, GoingObject> tmpDic = new Dictionary<string, GoingObject>();

            foreach (var tmp in lstGoingObj)
            {
                if (tmp.Value.objGoingType.Id == goingTypeId)
                {
                    tmpDic.Add(tmp.Key, tmp.Value);
                }
            }
            return tmpDic;
        }
        public ItemGoingWith GetItemGoingWithById(string id)
        {
            return lstItemGoingWith[id];
        }
        public Dictionary<string, ItemGoingWith> GetAllItemGoingWith()
        {
            return lstItemGoingWith;
        }
        public Dictionary<string, ItemGoingWith> GetItemGoingWithByItem(string itemId,bool includeGroups)
        {
            Dictionary<string, ItemGoingWith> tmpDic = new Dictionary<string, ItemGoingWith>();

            if (includeGroups)
            {
                foreach (var tmp in lstItemGoingWith)
                {
                    if (tmp.Value.Item.Id == itemId)
                    {
                        bool hasData = false;
                        foreach (var _tmpDic in tmpDic)
                        {
                            if (_tmpDic.Key == tmp.Key)
                            {
                                hasData = true;
                                break;
                            }
                        }

                        if (!hasData)
                        {
                            tmpDic.Add(tmp.Key, tmp.Value);
                        }                        
                    }
                }
            }
            else
            {
                foreach (var tmp in lstItemGoingWith)
                {
                    if (tmp.Value.Item.Id == itemId && Type.GetType(tmp.Value.GoingType.itmFunc.Key).Equals((new ItemGoing()).GetType()))
                    {
                        bool hasData = false;
                        foreach (var _tmpDic in tmpDic)
                        {
                            if (_tmpDic.Key == tmp.Key)
                            {
                                hasData = true;
                                break;
                            }
                        }

                        if (!hasData)
                        {
                            tmpDic.Add(tmp.Key, tmp.Value);
                        }
                    }
                }
            }
            
            return tmpDic;
        }
      
        public Dictionary<string, ItemGoingWith> GetItemGoingWithByGoingType(string goingTypeId)
        {
            Dictionary<string, ItemGoingWith> tmpDic = new Dictionary<string, ItemGoingWith>();

            foreach (var tmp in lstItemGoingWith)
            {
                if (tmp.Value.GoingType.Id == goingTypeId)
                {
                    tmpDic.Add(tmp.Key, tmp.Value);
                }
            }
            return tmpDic;
        }

        public Dictionary<string, ItemGoingWith> GetItemGoingWithByGoingObj(string goingObjId)
        {
            Dictionary<string, ItemGoingWith> tmpDic = new Dictionary<string, ItemGoingWith>();

            foreach (var tmp in lstItemGoingWith)
            {
                if (tmp.Value.GoingObj.Id == goingObjId)
                {
                    tmpDic.Add(tmp.Key, tmp.Value);
                }
            }
            return tmpDic;
        }

        public List<ObjectGoingWith> GetAllObjGoingWithFromGroupGoing(string goingObjWithId)
        {
            foreach (var tmp in lstGoingObj)
            {
                if (tmp.Key == goingObjWithId)
                {
                    if (Type.GetType(tmp.Value.objGoingType.itmFunc.Key).Equals(new GroupGoing().GetType()))
                    {
                        GroupGoing g = tmp.Value.objGoing as GroupGoing;
                        return g.GetLstObjGoingWith();
                    }
                    else
                    {
                        return new List<ObjectGoingWith>() { tmp.Value.objGoing };
                    }
                    
                }
            }
            return null;
        }
        public Dictionary<string, Status> GetAllStatus()
        {
            return lstStatus;
        }
        public Status GetStatusById(string id)
        {
            try
            {
                return lstStatus[id];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dictionary<string, Period> GetAllPeriod()
        {
            return lstPeriod;
        }
        public Period GetPeriodById(string id)
        {
            try
            {
                return lstPeriod[id];
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public Period GetPeriodByStatusId(string statusId)
        {
            foreach (var tmp in lstPeriod)
            {
                if (tmp.Value.Status.Id == statusId)
                {
                    return tmp.Value;
                }
            }
            return null;
        }

        public Dictionary<string, Transaction> GetAllTransaction()
        {
            return lstTransaction;
        }
        public Transaction GetTranById(string id)
        {
            return lstTransaction[id];
        }
        public List<Transaction> GetTranByTime(DateTime sDate, DateTime eDate)
        {
            List<Transaction> lstTran = new List<Transaction>();

            foreach (var tmp in lstTran)
            {
                if (tmp.CreatedDate >= sDate && tmp.CreatedDate <= eDate)
                {
                    lstTran.Add(tmp);
                }
            }

            return lstTran;
        }
        public Dictionary<string, StandardItemPrice> GetStdItemPrice()
        {
            return lstStdItmPrice;
        }
        public StandardItemPrice GetStdItmPriceById(string id)
        {
            return lstStdItmPrice[id];
        }

        public List<StandardItemPrice> GetStdItmPriceByItemId(string itemId)
        {
            List<StandardItemPrice> lstStdItemPrice = new List<StandardItemPrice>();
            foreach (var tmp in lstStdItmPrice)
            {
                if (tmp.Value.Item.Id == itemId)
                {
                    lstStdItemPrice.Add(tmp.Value);
                }
            }
            return lstStdItemPrice;
        }
        public List<StandardItemPrice> GetStdItmPriceByPeriodId(string periodId)
        {
            List<StandardItemPrice> lstStdItemPrice = new List<StandardItemPrice>();
            foreach (var tmp in lstStdItmPrice)
            {
                if (tmp.Value.Period.Id == periodId)
                {
                    lstStdItemPrice.Add(tmp.Value);
                }
            }
            return lstStdItemPrice;
        }

        public Dictionary<string, StandardItemProfile> GetStdItemProfile()
        {
            return lstStdItmProfile;
        }
        public StandardItemProfile GetStdItmProfById(string id)
        {
            return lstStdItmProfile[id];
        }
        public List<StandardItemProfile> GetStdItmProfByItemId(string itemId)
        {
            List<StandardItemProfile> lstStdItemProfile = new List<StandardItemProfile>();
            foreach (var tmp in lstStdItmProfile)
            {
                if (tmp.Value.Item.Id ==itemId)
                {
                    lstStdItemProfile.Add(tmp.Value);
                }
            }
            return lstStdItemProfile;
        }
        public List<StandardItemProfile> GetStdItmProfByPeriodId(string periodId)
        {
            List<StandardItemProfile> lstStdItemProfile = new List<StandardItemProfile>();
            foreach (var tmp in lstStdItmProfile)
            {
                if (tmp.Value.Period.Id == periodId)
                {
                    lstStdItemProfile.Add(tmp.Value);
                }
            }
            return lstStdItemProfile;
        }
        public Dictionary<string, DIYPriceType> GetDIYPriceType()
        {
            return lstDIYPriceType;
        }
        public DIYPriceType GetDIYPriceTypeById(string id)
        {
            return lstDIYPriceType[id];
        }
        public Dictionary<string, DIYProfile> GetDIYProfile()
        {
            return lstDIYProfile;
        }
        public DIYProfile GetDIYProfileById(string id)
        {
            return lstDIYProfile[id];
        }
        public DIYProfile GetDIYProfileByItemId(string itemId)
        {
            foreach (var tmp in lstDIYProfile)
            {
                if (tmp.Value.Item.Id == itemId)
                {
                    return tmp.Value;
                }
            }
            return null;
        }
        public DIYProfile GetDIYProfileByPeriodId(string periodId)
        {
            foreach (var tmp in lstDIYProfile)
            {
                if (tmp.Value.Period.Id == periodId)
                {
                    return tmp.Value;
                }
            }
            return null;
        }
        public DIYProfile GetDIYProfileByPriceTypeId(string priceTypeId)
        {
            foreach (var tmp in lstDIYProfile)
            {
                if (tmp.Value.PriceType.Id == priceTypeId)
                {
                    return tmp.Value;
                }
            }
            return null;
        }
        public Dictionary<string, DIYDefinition> GetDIYDefinition()
        {
            return lstDIYDef;
        }
        public DIYDefinition GetDIYDefinitionById(string id)
        {
            return lstDIYDef[id];
        }
        public List<DIYDefinition> GetDIYDefinitionByItemId(string itemId)
        {
            List<DIYDefinition> lstDef = new List<DIYDefinition>();
            foreach (var tmp in lstDIYDef)
            {
                if (tmp.Value.Item.Id == itemId)
                {
                    lstDef.Add(tmp.Value);
                }
            }
            return lstDef;
        }
        public List<DIYDefinition> GetDIYDefinitionByProfileId(string profileId)
        {
            List<DIYDefinition> lstDef = new List<DIYDefinition>();
            foreach (var tmp in lstDIYDef)
            {
                if (tmp.Value.DIYProfile.Id == profileId)
                {
                    lstDef.Add(tmp.Value);
                }
            }
            return lstDef;
        }
        public Dictionary<string, DIYFixedPrice> GetDIYFixedPrice()
        {
            return lstDIYFixedPrice;
        }
        public DIYFixedPrice GetDIYFixedPriceById(string id)
        {
            return lstDIYFixedPrice[id];
        }

        public Dictionary<string, DIYFixedPrice> GetDIYFixedPriceByPeriodId(string periodId)
        {
            Dictionary<string, DIYFixedPrice> lstTmp = new Dictionary<string, DIYFixedPrice>();

            foreach (var tmp in lstDIYFixedPrice)
            {
                if (tmp.Value.Period.Id == periodId)
                {
                    lstTmp.Add(tmp.Key, tmp.Value);
                }
            }

            return lstTmp;
        }

        public DIYFixedPrice GetDIYFPByProfIdWithActivePer(string profileId)
        {
            List<DIYFixedPrice> lstFixedPrice = new List<DIYFixedPrice>();
            foreach (var tmp in lstDIYFixedPrice)
            {
                if (tmp.Value.DIYProfile.Id == profileId && tmp.Value.Period.Status.StatusName == "Active")
                {
                    return tmp.Value;
                }
            }
            return null;
        }

        public List<DIYFixedPrice> GetDIYFixedPriceByProfileId(string profileId)
        {
            List<DIYFixedPrice> lstFixedPrice = new List<DIYFixedPrice>();
            foreach (var tmp in lstDIYFixedPrice)
            {
                if (tmp.Value.DIYProfile.Id == profileId)
                {
                    lstFixedPrice.Add(tmp.Value);
                }
            }
            return lstFixedPrice;
        }

        public Dictionary<string, DIYPrice> GetDIYPrice()
        {
            return lstDIYPrice;
        }
        public DIYPrice GetDIYPriceById(string id)
        {
            return lstDIYPrice[id];
        }
        public DIYPrice GetDIYPriceByTranId(string tranId)
        {
            foreach (var tmp in lstDIYPrice)
            {
                if (tmp.Value.TranId == tranId)
                {
                    return tmp.Value;
                }
            }

            return null;
        }
        public List<DIYPrice> GetDIYPriceByProfileId(string profileId)
        {
            List<DIYPrice> lstTmpPrice = new List<DIYPrice>();
            foreach (var tmp in lstDIYPrice)
            {
                if (tmp.Value.DIYProfile.Id == profileId)
                {
                    lstTmpPrice.Add(tmp.Value);
                }
            }

            return lstTmpPrice;
        }

        public DIYPrice GetDIYPriceBySaleTypeId(string saleTypeId)
        {
            foreach (var tmp in lstDIYPrice)
            {
                if (tmp.Value.DIYSelectionType.Id == saleTypeId)
                {
                    return tmp.Value;
                }
            }

            return null;
        }

        public Dictionary<string, DIYSelectionType> GetDIYSelectionType()
        {
            return lstDIYSelectType;
        }
        public DIYSelectionType GetDIYSelectionTypeById(string id)
        {
            return lstDIYSelectType[id];
        }
        public Dictionary<string, DIYSelection> GetDIYSelection()
        {
            return lstDIYSelection;
        }
        public DIYSelection GetDIYSelectionById(string id)
        {
            return lstDIYSelection[id];
        }

        public DIYSelection GetDIYSelectionByItemId(string itemId)
        {
            foreach (var tmp in lstDIYSelection)
            {
                if (tmp.Value.Item.Id == itemId)
                {
                    return tmp.Value;
                }
            }
            return null;
        }
        public DIYSelection GetDIYSelectionByDefId(string defId)
        {
            foreach (var tmp in lstDIYSelection)
            {
                if (tmp.Value.DIYDefinition.Id == defId)
                {
                    return tmp.Value;
                }
            }
            return null;
        }

        public DIYSelection GetDIYSelectionByPriceId(string priceId)
        {
            foreach (var tmp in lstDIYSelection)
            {
                if (tmp.Value.DIYPrice.Id == priceId)
                {
                    return tmp.Value;
                }
            }
            return null;
        }

        public void LoadData()
        {
            #region Connection
            SqlConnection con = new SqlConnection(this.ConnectionString);
            try
            {
                con.Open();
            }
            catch (Exception)
            {
                throw new Exception("Failure in opening a connection.");
            }
            #endregion

            #region Clear list

            lstItems.Clear();
            lstItemTypes.Clear();
            lstLanguages.Clear();
            lstItemData.Clear();
            //lstItemDataByItem.Clear();
            //lstItemDataByLang.Clear();
            lstRelatedItems.Clear();

            lstGroup.Clear();
            lstGroupItem.Clear();
            lstGroupTreeNode.Clear();

            lstItemTag.Clear();
            lstItemTagHis.Clear();
            lstTag.Clear();
            lstTagType.Clear();
            lstTagTypeCon.Clear();

            lstItemFunctions.Clear();
            lstGoingType.Clear();
            lstItemGoing.Clear();
            lstGroupGoing.Clear();
            lstObjGoingWith.Clear();
            lstGoingObj.Clear();
            lstItemGoingWith.Clear();

            lstPeriod.Clear();
            lstTransaction.Clear();
            lstStdItmPrice.Clear();
            lstStdItmProfile.Clear();
            lstDIYDef.Clear();
            lstDIYFixedPrice.Clear();
            lstDIYPrice.Clear();
            lstDIYPriceType.Clear();
            lstDIYProfile.Clear();
            lstDIYSelection.Clear();
            lstDIYSelectType.Clear();
            lstStatus.Clear();

            #endregion

            #region Get Data from DB

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            #region Get Data from SQL Server and put them in Dataset with different keys to illustrate each table-owned data

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * From tblItems"; // Get Data from tblItem

            da.SelectCommand = cmd;
            da.Fill(ds, "tblItems");

            cmd.CommandText = "Select * From tblItemTypes"; // Get Data from tblItemType
            da.Fill(ds, "tblItemTypes");

            cmd.CommandText = "Select * From tblLanguages"; // Get Data from tblLanguages
            da.Fill(ds, "tblLanguages");

            cmd.CommandText = "Select * From tblItemData"; // Get Data from tblItemData
            da.Fill(ds, "tblItemData");

            cmd.CommandText = "Select * From tblRelatedItems"; // Get Data from tblRelatedItems
            da.Fill(ds, "tblRelatedItems");

            cmd.CommandText = "Select * From tblGroups"; // Get Data from tblGroups
            da.Fill(ds, "tblGroups");

            cmd.CommandText = "Select * From tblGroupItems"; // Get Data from tblGroupItems
            da.Fill(ds, "tblGroupItems");

            cmd.CommandText = "Select * From tblGroupTreeNode"; // Get Data from GroupTreeNode
            da.Fill(ds, "tblGroupTreeNode");

            cmd.CommandText = "Select * From tblTagType"; // Get Data from tblTagType
            da.Fill(ds, "tblTagType");

            cmd.CommandText = "Select * From tblTagTypeConstraint"; // Get Data from tblTagTypeConstraint
            da.Fill(ds, "tblTagTypeConstraint");

            cmd.CommandText = "Select * From tblTag"; // Get Data from tblTag
            da.Fill(ds, "tblTag");

            cmd.CommandText = "Select * From tblItemTag"; // Get Data from tblItemTag
            da.Fill(ds, "tblItemTag");

            cmd.CommandText = "Select * From tblItemTagHistory"; // Get Data from tblItemTagHistory
            da.Fill(ds, "tblItemTagHistory");



            cmd.CommandText = "Select * From tblItemFunctions"; // Get Data from tblItemFunctions
            da.Fill(ds, "tblItemFunctions");

            cmd.CommandText = "Select * From tblGoingType"; // Get Data from tblGoingType
            da.Fill(ds, "tblGoingType");

            cmd.CommandText = "Select * From tblGroupGoing"; // Get Data from tblGroupGoing
            da.Fill(ds, "tblGroupGoing");

            cmd.CommandText = "Select * From tblItemGoing"; // Get Data from tblItemGoing
            da.Fill(ds, "tblItemGoing");

            cmd.CommandText = "Select * From tblGoingObject"; // Get Data from tblGoingObject
            da.Fill(ds, "tblGoingObject");

            cmd.CommandText = "Select * From tblItemGoingWith"; // Get Data from tblItemGoingWith
            da.Fill(ds, "tblItemGoingWith");


            cmd.CommandText = "Select * From tblTransaction"; // Get Data from tblTransaction
            da.Fill(ds, "tblTransaction");

            cmd.CommandText = "Select * From tblStatus"; // Get Data from tblStatus
            da.Fill(ds, "tblStatus");

            cmd.CommandText = "Select * From tblPeriod"; // Get Data from tblPeriod
            da.Fill(ds, "tblPeriod");

            cmd.CommandText = "Select * From tblStandardItemPrice"; // Get Data from tblStandardItemPrice
            da.Fill(ds, "tblStandardItemPrice");

            cmd.CommandText = "Select * From tblStandardItemProfile"; // Get Data from tblStandardItemProfile
            da.Fill(ds, "tblStandardItemProfile");

            cmd.CommandText = "Select * From tblDIYPriceType"; // Get Data from tblDIYPriceType
            da.Fill(ds, "tblDIYPriceType");

            cmd.CommandText = "Select * From tblDIYProfile"; // Get Data from tblDIYProfile
            da.Fill(ds, "tblItemGoingWith");

            cmd.CommandText = "Select * From tblDIYDefinition"; // Get Data from tblDIYDefinition
            da.Fill(ds, "tblDIYDefinition");

            cmd.CommandText = "Select * From tblDIYFixedPrice"; // Get Data from tblDIYFixedPrice
            da.Fill(ds, "tblDIYFixedPrice");

            cmd.CommandText = "Select * From tblDIYSelectionType"; // Get Data from tblDIYSelectionType
            da.Fill(ds, "tblDIYSelectionType");

            cmd.CommandText = "Select * From tblDIYPrice"; // Get Data from tblDIYPrice
            da.Fill(ds, "tblDIYPrice");

            cmd.CommandText = "Select * From tblDIYSelection"; // Get Data from tblDIYSelection
            da.Fill(ds, "tblDIYSelection");

            cmd.CommandText = "Select * From tblDIYProfile"; // Get Data from tblDIYProfile
            da.Fill(ds, "tblDIYProfile");

            #endregion

            #region Get Data of tblItemTypes from DataTable and put them in Dictionary

            DataTable dtItemTypes = ds.Tables["tblItemTypes"]; // Get Data from DataTable where key = tblItemTypes
            foreach (DataRow dr in dtItemTypes.Rows)
            {
                ItemType itmType = new ItemType()
                {
                    Id = Convert.ToString(dr["Id"]),
                    Name = Convert.ToString(dr["Name"]),
                    Description = Convert.ToString(dr["Description"])
                };
                lstItemTypes.Add(itmType.Id, itmType);
            }

            #endregion

            #region Get Data tblLanguages from DataTable and put them in Dictionary

            DataTable dtLanguages = ds.Tables["tblLanguages"]; // Get Data from DataTable where key = tblLanguages
            foreach (DataRow dr in dtLanguages.Rows)
            {
                string tempId = Convert.ToString(dr["Id"]);
                Language lang = new Language()
                {
                    Id = tempId,
                    Name = Convert.ToString(dr["Name"]),
                    Description = Convert.ToString(dr["Description"]),
                    Priority = Convert.ToInt32(dr["Priority"])
                    //LstItemData = lstItemData.Values.Where(r=> r.Language.Id == tempId).ToList()
                };
                lstLanguages.Add(lang.Id, lang);
            }

            #endregion

            #region Get Data tblItem  from DataTable and put them in Dictionary
            //   List<clsItemComponent> lstItems = new List<clsItemComponent>();            

            DataTable dtItem = ds.Tables["tblItems"]; // Get Data from DataTable where key = tblItem
            foreach (DataRow dr in dtItem.Rows)
            {
                #region Get ItemType for Item

                var getItemType = lstItemTypes.First(r => r.Key == Convert.ToString(dr["ItemTypeId"])).Value;

                #endregion                               

                Item objTemp = new Item()
                {
                    Id = Convert.ToString(dr["Id"]),
                    Description = Convert.ToString(dr["Description"]),
                    ItemType = getItemType,
                    Code = Convert.ToString(dr["Code"]),
                    Name = "",
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    CountBuy = 0,
                    CountLike = 0
                };

                lstItems.Add(objTemp);
                //lstItems.Add((ItemComposite)objTemp);
            }


            #region Get SubItems And Put It In ParentItem
            //foreach (var item in lstItems)
            //{
            //    var getChildrenItems = lstMappingItems.Where(r => r.ParentId == item.Key); // Get children item
            //    foreach (var subitem in getChildrenItems)
            //    {
            //        try
            //        {
            //            var getSubItems = lstItems.Where(r => r.Key == subitem.ChildId);
            //            if (getSubItems.Count() > 0)
            //            {
            //                item.Value.lstSubItems.Add(getSubItems.First().Value);
            //            }
            //        }
            //        catch (Exception)
            //        {

            //            throw;
            //        }
            //    }
            //}            

            #endregion
            #endregion

            #region Get Data of tblItemData from DataTable and put them in Dictionary 

            DataTable dtItemData = ds.Tables["tblItemData"]; // Get Data from DataTable where key = tblItemData
            foreach (DataRow dr in dtItemData.Rows)
            {
                //string getPriorityName = 
                ItemData itemData = new ItemData()
                {
                    Id = Convert.ToString(dr["Id"]),
                    Item = lstItems.First(r => r.Id == Convert.ToString(dr["ItemId"])),
                    Language = lstLanguages.First(r => r.Key == Convert.ToString(dr["LanguageId"])).Value,
                    LanguageTextItem = Convert.ToString(dr["LanguageTextItem"]),
                    InUse = Convert.ToBoolean(dr["InUse"])
                };
                //lstItemDataByItem.Add(itemData.ItemId, itemData);
                //lstItemDataByLang.Add(itemData.LanguageId, itemData);

                lstItemData.Add(itemData.Id, itemData);
            }

            #endregion

            #region Put display Name in items
            foreach (var item in lstItems)
            {

                var getItemDisLan = lstItemData.Where(r => r.Value.Item.Id == item.Id).OrderBy(r => r.Value.Language.Priority).First();

                item.Name = getItemDisLan.Value.LanguageTextItem;
                getItemDisLan.Value.InUse = true;
            }
            #endregion

            #region Get Data tblRelatedItems from DataTable and put them in Dictionary

            DataTable dtRelatedItems = ds.Tables["tblRelatedItems"]; // Get Data from DataTable where key = tblRelatedItems
            foreach (DataRow dr in dtRelatedItems.Rows)
            {
                RelatedItems relatedItem = new RelatedItems()
                {
                    Id = Convert.ToString(dr["Id"]),
                    ParentItem = lstItems.First(r => r.Id == Convert.ToString(dr["ItemParentId"])), // Convert.ToString(dr["ItemParentId"]),
                    Item = lstItems.First(r => r.Id == Convert.ToString(dr["ItemChildId"])) // Convert.ToString(dr["ItemChildId"])
                };
                lstRelatedItems.Add(relatedItem);
            }

            #endregion                            

            #region Get Data tblGroup from DataTable and put them in Dictionary

            DataTable dtGroup = ds.Tables["tblGroups"]; // Get Data from DataTable where key = tblGroups
            foreach (DataRow dr in dtGroup.Rows)
            {
                Group group = new Group()
                {
                    Id = Convert.ToString(dr["Id"]),
                    Name = Convert.ToString(dr["Name"])
                };
                lstGroup.Add(group.Id, group);
            }

            #endregion

            #region Get Data tblGroupItems from DataTable and put them in Dictionary

            DataTable dtGroupItems = ds.Tables["tblGroupItems"]; // Get Data from DataTable where key = tblGroupItems
            foreach (DataRow dr in dtGroupItems.Rows)
            {
                string groupItemId = Convert.ToString(dr["Id"]);

                #region Get Item

                string itemId = Convert.ToString(dr["ItemId"]);
                var getItems = lstItems.First(r => r.Id == itemId);

                #endregion

                #region Get Group

                string groupId = Convert.ToString(dr["GroupId"]);
                var getGroup = lstGroup[groupId];

                #endregion

                GroupItem groupItem = new GroupItem()
                {
                    Id = groupItemId,
                    Item = getItems,
                    Group = getGroup
                };
                lstGroupItem.Add(groupItem.Id, groupItem);
            }

            #endregion

            #region Get Data tblGroupTreeNode from DataTable and put them in Dictionary

            DataTable dtGroupTreeNode = ds.Tables["tblGroupTreeNode"]; // Get Data from DataTable where key = tblGroupTreeNode
            foreach (DataRow dr in dtGroupTreeNode.Rows)
            {
                GroupTreeNode groupTreeNode = new GroupTreeNode()
                {
                    Id = Convert.ToString(dr["Id"]),
                    Group = lstGroup.First(r => r.Key == Convert.ToString(dr["GroupId"])).Value,
                    ParentGroup = dr["ParentGroupId"] == null || Convert.ToString(dr["ParentGroupId"]) == "" ? null : lstGroup.First(r => r.Key == Convert.ToString(dr["ParentGroupId"])).Value
                };
                lstGroupTreeNode.Add(groupTreeNode.Id, groupTreeNode);
            }

            #endregion

            #region Get Data tblTagType from DataTable and put them in Dictionary

            DataTable dtTagType = ds.Tables["tblTagType"]; // Get Data from DataTable where key = tblTagType
            foreach (DataRow dr in dtTagType.Rows)
            {
                TagType tagType = new TagType()
                {
                    Id = Convert.ToString(dr["Id"]),
                    Name = Convert.ToString(dr["Name"])
                };
                lstTagType.Add(tagType.Id, tagType);
            }

            #endregion

            #region Get Data tblTagTypeConstraint from DataTable and put them in Dictionary

            DataTable dtTagTypeConstraint = ds.Tables["tblTagTypeConstraint"]; // Get Data from DataTable where key = tblTagTypeConstraint
            foreach (DataRow dr in dtTagTypeConstraint.Rows)
            {
                var getTagType = GetTagTypeById(Convert.ToString(dr["TagTypeId"]));
                TagTypeConstraint tagType = new TagTypeConstraint(Convert.ToString(dr["Id"]), Convert.ToString(dr["Name"]), getTagType);
                lstTagTypeCon.Add(tagType.Id, tagType);
            }

            #endregion


            #region Get Data tblTag from DataTable and put them in Dictionary

            DataTable dtTag = ds.Tables["tblTag"]; // Get Data from DataTable where key = tblTag
            foreach (DataRow dr in dtTag.Rows)
            {
                var tmpTagType = GetTagTypeById(Convert.ToString(dr["TagTypeId"]));

                TagTypeConstraint tmpCon = GetTagTypeConById(Convert.ToString(dr["TagTypeConstraintId"]));

                Tag tag = new Tag(Convert.ToString(dr["Id"]), tmpTagType, tmpCon);
                lstTag.Add(tag.Id, tag);
            }

            #endregion

            #region Get Data tblItemTag from DataTable and put them in Dictionary

            DataTable dtItemTag = ds.Tables["tblItemTag"]; // Get Data from DataTable where key = tblItemTag
            foreach (DataRow dr in dtItemTag.Rows)
            {
                var getItem = GetItemById(Convert.ToString(dr["ItemId"]));
                var getTag = GetTagById(Convert.ToString(dr["TagId"]));
                ItemTag itemTag = new ItemTag(getItem, getTag);
                itemTag.Id = Convert.ToString(dr["Id"]);

                lstItemTag.Add(itemTag.Id, itemTag);
            }

            #endregion

            #region Get Data tblItemTagHistory from DataTable and put them in Dictionary

            DataTable dtItemTagHis = ds.Tables["tblItemTagHistory"]; // Get Data from DataTable where key = tblItemTagHistory
            foreach (DataRow dr in dtItemTagHis.Rows)
            {
                var getItem = GetItemById(Convert.ToString(dr["ItemId"]));
                var getTagType = GetTagTypeById(Convert.ToString(dr["TagTypeId"]));
                var getTagTypeCon = GetTagTypeConById(Convert.ToString(dr["TagTypeConstraintId"]));

                ItemTagHistory obj = new ItemTagHistory(getItem, getTagType, getTagTypeCon, Convert.ToDateTime(dr["StartDate"]));
                obj.ItemTag = GetItemTagById(Convert.ToString(dr["ItemTagId"]));
                obj.EndDate = Convert.ToString(dr["EndDate"]) == "" ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                obj.Note = Convert.ToString(dr["Note"]);

                lstItemTagHis.Add(Convert.ToString(dr["ItemTagId"]), obj);


            }

            #endregion



            #region Get Data tblItemFunction from DataTable and put them in Dictionary

            DataTable dtItemFun = ds.Tables["tblItemFunctions"]; // Get Data from DataTable where key = tblItemFunction
            foreach (DataRow dr in dtItemFun.Rows)
            {
                ItemFunction obj = new ItemFunction();
                obj.Id = Convert.ToString(dr["Id"]);
                obj.Name = Convert.ToString(dr["Name"]);
                obj.Key = Convert.ToString(dr["Key"]);

                lstItemFunctions.Add(obj.Id, obj);
            }

            #endregion

            #region Get Data tblItemGoing from DataTable and put them in Dictionary

            DataTable dtItemGoing = ds.Tables["tblItemGoing"]; // Get Data from DataTable where key = tblItemGoing
            foreach (DataRow dr in dtItemGoing.Rows)
            {
                //var getGoingObj = GetGoingObjectById(Convert.ToString(dr["GoingObjId"]));
                GoingObject goingobj = new GoingObject();
                goingobj.Id = Convert.ToString(dr["GoingObjId"]);

                var getItem = GetItemById(Convert.ToString(dr["ItemId"]));
                ItemGoing obj = new ItemGoing();
                obj.Id = getItem.Id;
                obj.Name = getItem.Name;
                obj.ItemType = getItem.ItemType;
                obj.ItemTag = getItem.ItemTag;
                obj.Visited = false;
                obj.Code = getItem.Code;
                obj.CreatedDate = getItem.CreatedDate;
                obj.Description = getItem.Description;
                obj.GoingObj = goingobj;

                lstItemGoing.Add(obj.Id, obj);
            }

            #endregion

            #region Get Data tblGroupGoing from DataTable and put them in Dictionary

            DataTable dtGroupGoing = ds.Tables["tblGroupGoing"]; // Get Data from DataTable where key = tblGroupGoing
            foreach (DataRow dr in dtGroupGoing.Rows)
            {
                GoingObject goingobj = new GoingObject();
                goingobj.Id = Convert.ToString(dr["GoingObjId"]);

                var getGroup = GetGroupById(Convert.ToString(dr["GroupId"]));

                GroupGoing obj = new GroupGoing();
                obj.Id = getGroup.Id;
                obj.Name = getGroup.Name;
                obj.Visited = false;
                obj.GoingObj = goingobj;

                lstGroupGoing.Add(obj.Id, obj);
            }

            #endregion

            #region Get Data tblGoingType from DataTable and put them in Dictionary

            DataTable dtGroupType = ds.Tables["tblGoingType"]; // Get Data from DataTable where key = tblGoingType
            foreach (DataRow dr in dtGroupType.Rows)
            {
                var getItemFunc = GetItemFunctionById(Convert.ToString(dr["ItemFunId"]));
                GoingType obj = new GoingType(
                        Convert.ToString(dr["Id"]),
                        Convert.ToString(dr["Name"]),
                        getItemFunc
                    );

                lstGoingType.Add(obj.Id, obj);
            }

            #endregion
            
            #region Get Data tblGoingObject from DataTable and put them in Dictionary

            DataTable dtGoingObject = ds.Tables["tblGoingObject"]; // Get Data from DataTable where key = tblGoingObject
            foreach (DataRow dr in dtGoingObject.Rows)
            {
                ObjectGoingWith tmpGoingWith = null;
                var getGoingType = GetGoingTypeById(Convert.ToString(dr["GoingTypeId"]));
                switch (getGoingType.Name)
                {
                    case "Group":
                        var getGroupGoing = GetGroupGoingById(Convert.ToString(dr["ObjGoingWith"]));
                        var getGroupItems = GetGroupItemByGroup(getGroupGoing); // check group items
                        var getGroupTree = GetGroupTreeNodeByParentId(Convert.ToString(dr["ObjGoingWith"])); // check sub groups

                        HashSet<ObjectGoingWith> lstVisitedItems = new HashSet<ObjectGoingWith>();

                        Depth_First_Traversal_Graph d = new Depth_First_Traversal_Graph();

                        foreach (var tmp in d.findGroupItems(GetAllGoingType, GetItemGoingById, getGroupItems))
                        {
                            lstVisitedItems.Add(tmp);
                        }

                        foreach (var tmp in d.getGroupTree(GetAllGoingType, GetItemGoingById, GetGroupGoingById, GetGroupItemByGroup, GetGroupTreeNodeByParentId, getGroupTree, lstVisitedItems))
                        {
                            lstVisitedItems.Add(tmp);
                        }

                        
                        //lstVisitedItems.Concat(d.findGroupItems(GetItemGoingById, getGroupItems)).ToList();


                        //lstVisitedItems.Concat(d.getGroupTree(GetItemGoingById, GetGroupGoingById, GetGroupItemByGroup, GetGroupTreeNodeByParentId, getGroupTree)).ToList();

                        foreach (var tmp in lstVisitedItems)
                        {
                            getGroupGoing.AddItemGoing(tmp);
                        }

                        #region Old Codes
                        //foreach (var tmp in getGroupItems)
                        //{
                        //    var tmpItmGoing = GetItemGoingById(tmp.Item.Id);
                        //    if (!getGroupGoing.CheckVisitedNode(tmpItmGoing))
                        //    {
                        //        tmpItmGoing.Visited = true;
                        //        getGroupGoing.AddItemGoing(tmpItmGoing);
                        //    }                           
                        //}

                        //foreach (var tmpTree in getGroupTree)
                        //{
                        //    var tmpGroupGoing = GetGroupGoingById(tmpTree.Group.Id);
                        //    if (!getGroupGoing.CheckVisitedNode(tmpGroupGoing))
                        //    {
                        //        tmpGroupGoing.Visited = true;
                        //        getGroupGoing.AddItemGoing(tmpGroupGoing);
                        //    }
                        //}
                        #endregion

                        tmpGoingWith = getGroupGoing;
                        break;

                    case "Item":
                        var getItemGoing = GetItemGoingById(Convert.ToString(dr["ObjGoingWith"]));
                        //GroupGoing tmpGroup = new GroupGoing();
                        //tmpGroup.AddItemGoing(getItemGoing);

                        
                        tmpGoingWith = getItemGoing; break;
                    default: break;
                }

                GoingObject obj = new GoingObject(
                        Convert.ToString(dr["Id"]),
                        getGoingType,
                        tmpGoingWith
                );

                lstGoingObj.Add(obj.Id, obj);
            }

            #endregion

            #region Get Data tblItemGoingWith from DataTable and put them in Dictionary

            DataTable dtItemGoingWith = ds.Tables["tblItemGoingWith"]; // Get Data from DataTable where key = tblItemGoingWith
            foreach (DataRow dr in dtItemGoingWith.Rows)
            {
                var getItem = GetItemById(Convert.ToString(dr["ItemId"]));
                var getGoingType = GetGoingTypeById(Convert.ToString(dr["GoingTypeId"]));
                var getGoingObj = GetGoingObjectById(Convert.ToString(dr["GoingObjId"]));
                ItemGoingWith obj = new ItemGoingWith(
                        Convert.ToString(dr["Id"]),
                        getItem,
                        getGoingObj,
                        getGoingType
                    );

                lstItemGoingWith.Add(obj.Id, obj);
            }

            #endregion

            #region Update tblItemGoing
            foreach (var tmp in lstItemGoing)
            {
                tmp.Value.GoingObj = GetGoingObjectById(tmp.Value.GoingObj.Id);
            }
            #endregion

            #region Update tblGroupGoing
            foreach (var tmp in lstGroupGoing)
            {
                tmp.Value.GoingObj = GetGoingObjectById(tmp.Value.GoingObj.Id);
            }
            #endregion


            #region Get Data tblTransaction from DataTable and put them in Dictionary
            DataTable dtTransaction = ds.Tables["tblTransaction"]; // Get Data from DataTable where key = tblTransaction
            foreach (DataRow dr in dtTransaction.Rows)
            {
                Transaction obj = new Transaction()
                {
                    TranId = Convert.ToString(dr["Id"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"])
                };
                lstTransaction.Add(obj.TranId, obj);
            }
            #endregion

            #region Get Data tblStatus from DataTable and put them in Dictionary
            DataTable dtStatus = ds.Tables["tblStatus"]; // Get Data from DataTable where key = tblStatus
            foreach (DataRow dr in dtStatus.Rows)
            {
                Status obj = new Status();
                obj.Id = Convert.ToString(dr["Id"]);
                obj.StatusName = Convert.ToString(dr["StatusName"]);
                obj.Type = Convert.ToString(dr["Type"]);

                lstStatus.Add(obj.Id, obj);
            }
            #endregion

            #region Get Data tblPeriod from DataTable and put them in Dictionary
            DataTable dtPeriod = ds.Tables["tblPeriod"]; // Get Data from DataTable where key = tblPeriod
            foreach (DataRow dr in dtPeriod.Rows)
            {
                #region Get Status
                var status = GetStatusById(Convert.ToString(dr["Status"]));
                #endregion

                Period obj = new Period();
                obj.Id = Convert.ToString(dr["Id"]);
                obj.StartDate = Convert.ToDateTime(dr["StartDate"]);
                obj.Status = status;
                if (dr["EndDate"].ToString() == "")
                {
                    obj.EndDate = null;
                }
                else
                {
                    obj.EndDate = Convert.ToDateTime(dr["EndDate"]);
                }
                
                lstPeriod.Add(obj.Id, obj);
            }
            #endregion

            #region Get Data tblStandardItemPrice from DataTable and put them in Dictionary
            DataTable dtStdItemPrice = ds.Tables["tblStandardItemPrice"]; // Get Data from DataTable where key = tblStandardItemPrice
            foreach (DataRow dr in dtStdItemPrice.Rows)
            {
                #region GetItem
                var item = GetItemById(Convert.ToString(dr["ItemId"]));
                #endregion
                #region GetPeriod
                var period = GetPeriodById(Convert.ToString(dr["PeriodId"]));
                #endregion
                StandardItemPrice itmPrice = new StandardItemPrice(item, period);
                itmPrice.Id = Convert.ToString(dr["Id"]);
                itmPrice.StandardPrice = Convert.ToDouble(dr["StandardPrice"]);
                lstStdItmPrice.Add(itmPrice.Id, itmPrice);
            }
            #endregion

            #region Get Data tblStandardItemProfile from DataTable and put them in Dictionary
            DataTable dtStdItmProfile = ds.Tables["tblStandardItemProfile"]; // Get Data from DataTable where key = tblStandardItemProfile
            foreach (DataRow dr in dtStdItmProfile.Rows)
            {
                #region GetItem
                var item = GetItemById(Convert.ToString(dr["ItemId"]));
                #endregion
                #region GetPeriod
                var period = GetPeriodById(Convert.ToString(dr["PeriodId"]));
                #endregion
                StandardItemProfile itmProfile = new StandardItemProfile(item, period);
                itmProfile.Id = Convert.ToString(dr["Id"]);
                itmProfile.Description = Convert.ToString(dr["Description"]);
                lstStdItmProfile.Add(itmProfile.Id, itmProfile);
            }
            #endregion

            #region Get Data tblDIYPriceType from DataTable and put them in Dictionary
            DataTable dtDIYPriceType = ds.Tables["tblDIYPriceType"]; // Get Data from DataTable where key = tblDIYPriceType
            foreach (DataRow dr in dtDIYPriceType.Rows)
            {
                DIYPriceType diyPriceType = new DIYPriceType();
                diyPriceType.Id = Convert.ToString(dr["Id"]);
                diyPriceType.Name = Convert.ToString(dr["Name"]);
                lstDIYPriceType.Add(diyPriceType.Id, diyPriceType);
            }
            #endregion

            #region Get Data tblDIYProfile from DataTable and put them in Dictionary
            DataTable dtDIYProfile = ds.Tables["tblDIYProfile"]; // Get Data from DataTable where key = tblDIYProfile
            foreach (DataRow dr in dtDIYProfile.Rows)
            {
                #region Item
                var item = GetItemById(Convert.ToString(dr["ItemId"]));
                #endregion
                #region Period
                var period = GetPeriodById(Convert.ToString(dr["PeriodId"]));
                #endregion
                #region DIYPriceType
                var diyPriceType = GetDIYPriceTypeById(Convert.ToString(dr["DIYPriceTypeId"]));
                #endregion
                #region List DIYDefinition
                var diyDef = GetDIYDefinitionByProfileId(Convert.ToString(dr["Id"]));
                #endregion
                DIYProfile diyProfile = new DIYProfile(diyPriceType, item, period, diyDef);
                diyProfile.Id = Convert.ToString(dr["Id"]);
                diyProfile.MinChosen = Convert.ToDouble(dr["MinChosen"]);
                diyProfile.MinProportion = Convert.ToDouble(dr["MinProportion"]);
                lstDIYProfile.Add(diyProfile.Id, diyProfile);
            }
            #endregion

            #region Get Data tblDIYDefinition from DataTable and put them in Dictionary
            DataTable dtDIYDef = ds.Tables["tblDIYDefinition"]; // Get Data from DataTable where key = tblDIYDefinition
            foreach (DataRow dr in dtDIYDef.Rows)
            {
                #region GetItem
                var item = GetItemById(Convert.ToString(dr["ItemId"]));
                #endregion
                #region Profile
                var profile = GetDIYProfileById(Convert.ToString(dr["DIYProfileId"]));
                #endregion
                DIYDefinition diyDef = new DIYDefinition(profile, item);
                diyDef.Id = Convert.ToString(dr["Id"]);
                lstDIYDef.Add(diyDef.Id, diyDef);
            }
            #endregion
     
            #region Get Data tblDIYFixedPrice from DataTable and put them in Dictionary
            DataTable dtFixedPrice = ds.Tables["tblDIYFixedPrice"]; // Get Data from DataTable where key = tblDIYFixedPrice
            foreach (DataRow dr in dtFixedPrice.Rows)
            {
                #region Period
                var period = GetPeriodById(Convert.ToString(dr["PeriodId"]));
                #endregion
                DIYFixedPrice diyFixPrice = new DIYFixedPrice(period);
                diyFixPrice.Id = Convert.ToString(dr["Id"]);
                diyFixPrice.Value = Convert.ToDouble(dr["Value"]);
                diyFixPrice.DIYProfile = GetDIYProfileById(Convert.ToString(dr["DIYProfileId"]));
                lstDIYFixedPrice.Add(diyFixPrice.Id, diyFixPrice);
            }
            #endregion

            #region Get Data tblDIYSelectionType from DataTable and put them in Dictionary
            DataTable dtDIYSelType = ds.Tables["tblDIYSelectionType"]; // Get Data from DataTable where key = tblDIYSelectionType
            foreach (DataRow dr in dtDIYSelType.Rows)
            {
                DIYSelectionType diySelType = new DIYSelectionType();
                diySelType.Id = Convert.ToString(dr["Id"]);
                diySelType.Name = Convert.ToString(dr["Name"]);
                lstDIYSelectType.Add(diySelType.Id, diySelType);
            }
            #endregion

            #region Get Data tblDIYPrice from DataTable and put them in Dictionary
            DataTable dtDIYPrice = ds.Tables["tblDIYPrice"]; // Get Data from DataTable where key = tblDIYPrice
            foreach (DataRow dr in dtDIYPrice.Rows)
            {
                #region Transaction
                var tran = GetTranById(Convert.ToString(dr["TransactionId"]));
                #endregion
                #region DIYProfile
                var diyProfile = GetDIYProfileById(Convert.ToString(dr["DIYProfileId"]));
                #endregion
                #region DIYSelType
                var diySelType = GetDIYSelectionTypeById(Convert.ToString(dr["DIYSelectionTypeId"]));
                #endregion
                DIYPrice diyPrice = new DIYPrice(diyProfile, diySelType);
                diyPrice.Id = Convert.ToString(dr["Id"]);
                diyPrice.ValueDIY = Convert.ToDouble(dr["ValueDIY"]);
                lstDIYPrice.Add(diyPrice.Id, diyPrice);
            }
            #endregion

            #region Get Data tblDIYSelection from DataTable and put them in Dictionary
            DataTable dtDIYSelection = ds.Tables["tblDIYSelection"]; // Get Data from DataTable where key = tblDIYSelection
            foreach (DataRow dr in dtDIYSelection.Rows)
            {
                #region DIYPrice
                var price = GetDIYPriceById(Convert.ToString(dr["DIYPriceId"]));
                #endregion
                #region Item
                var item = GetItemById(Convert.ToString(dr["ItemId"]));
                #endregion
                #region DIYDefinition
                //var diyDef = GetDIYDefinitionById(Convert.ToString(dr["DIYDefinitionId"]));
                #endregion
                DIYSelection diySel = new DIYSelection(item, price);
                diySel.Id = Convert.ToString(dr["Id"]);
                diySel.ProportionVal = Convert.ToDouble(dr["ProportionVal"]);
                diySel.Price = Convert.ToDouble(dr["Price"]);
                lstDIYSelection.Add(diySel.Id, diySel);
            }
            #endregion

            #region Update lstDIYProfile
            foreach (var tmp in lstDIYProfile)
            {
                tmp.Value.lstDIYDefinitions = GetDIYDefinitionByProfileId(tmp.Key);
                tmp.Value.lstDIYFixedPrice = GetDIYFixedPriceByProfileId(tmp.Key);
                tmp.Value.lstDIYPrice = GetDIYPriceByProfileId(tmp.Key);
            }
            #endregion

            #endregion

        }

        public void InsertItem(Item item)
        {
            try
            {
                using (SqlConnection openCon = new SqlConnection(this.ConnectionString))
                {
                    string strSave = "INSERT into tblItems (id, ItemTypeId, code, Description, createdDate, Active) output INSERTED.Id VALUES (@id, @itemType, @code, @des, @createdDate, @active)";
                    string insertedItemId = null;
                    using (SqlCommand cmd = new SqlCommand(strSave))
                    {
                        cmd.Connection = openCon;
                        cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                        cmd.Parameters.Add("@itemType", SqlDbType.VarChar, 50).Value = item.ItemType.Id;
                        cmd.Parameters.Add("@code", SqlDbType.VarChar, 50).Value = item.Code;
                        cmd.Parameters.Add("@des", SqlDbType.NVarChar).Value = item.Description;
                        cmd.Parameters.Add("@createdDate", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@active", SqlDbType.Bit).Value = item.Active;

                        openCon.Open();

                        insertedItemId = cmd.ExecuteScalar().ToString();
                    }

                    foreach (ItemData itemData in item.lstItemData)
                    {
                        string strSaveItemData = "INSERT INTO tblItemData (Id, ItemId, LanguageId, LanguageTextItem, InUse) VALUES (@id, @itemId, @languageId, @langTextItem, @inUse)";
                        using (SqlCommand cmd = new SqlCommand(strSaveItemData))
                        { 
                            cmd.Connection = openCon;
                            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                            cmd.Parameters.Add("@itemId", SqlDbType.VarChar, 50).Value = insertedItemId;
                            cmd.Parameters.Add("@languageId", SqlDbType.VarChar, 50).Value = itemData.Language.Id;
                            cmd.Parameters.Add("@langTextItem", SqlDbType.NVarChar).Value = itemData.LanguageTextItem;
                            cmd.Parameters.Add("@inUse", SqlDbType.Bit).Value = itemData.InUse;
                            //openCon.Open();

                            cmd.ExecuteNonQuery();
                        }
                    }

                    DateTime tmpDatetime = DateTime.Now;
                    string insertedPeriodId = null; 

                    foreach (StandardItemProfile stdItmProf in item.lstStdItmProfile)
                    {

                        #region Insert into Period

                        //insertedPeriodId = this.InsertPeriod(stdItmProf.Period);
                        if (insertedPeriodId == null)
                        {
                            string strSavePeriod = "INSERT INTO tblPeriod (Id, StartDate, EndDate, Status) output INSERTED.Id VALUES (@id, @startDate, @endDate, @status)";
                            using (SqlCommand cmd = new SqlCommand(strSavePeriod))
                            {   
                                cmd.Connection = openCon;
                                cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = tmpDatetime;
                                //cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = stdItmProf.Period.StartDate;


                                if (stdItmProf.Period.EndDate == null)
                                {
                                    cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@endDate", stdItmProf.Period.EndDate.Value);
                                }

                                cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = "STS-001";

                                //openCon.Open();

                                insertedPeriodId = cmd.ExecuteScalar().ToString();
                            }
                        }

                        #endregion

                        string strSaveStdItmProfile = "INSERT INTO tblStandardItemProfile (Id, ItemId, PeriodId, Description) VALUES (@id, @itemId, @periodId, @description)";
                        using (SqlCommand cmd = new SqlCommand(strSaveStdItmProfile))
                        {
                            if (openCon == null || openCon.State != ConnectionState.Open)
                                openCon.Open();

                            cmd.Connection = openCon;
                            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                            cmd.Parameters.Add("@itemId", SqlDbType.VarChar, 50).Value = insertedItemId;
                            cmd.Parameters.Add("@periodId", SqlDbType.VarChar, 50).Value = insertedPeriodId;
                            cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = stdItmProf.Description == null? "" : stdItmProf.Description;
                            //openCon.Open();

                            cmd.ExecuteNonQuery();
                        }
                    }

                    #region For ItemType == Normal
                    foreach (StandardItemPrice stp in item.lstStdItemPrice)
                    {
                        #region Insert Into tblPeriod

                        //string insertedPeriodId = this.InsertPeriod(stp.Period);
                        //if (insertedPeriodId == null)
                        //{
                        //    string strSavePeriod = "INSERT INTO tblPeriod (Id, StartDate, EndDate) output INSERTED.Id VALUES (@id, @startDate, @endDate)";
                        //    using (SqlCommand cmd = new SqlCommand(strSavePeriod))
                        //    {
                        //        cmd.Connection = openCon;
                        //        cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                        //        cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = stp.Period.StartDate;
                        //        //cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = stp.Period.StartDate;


                        //        if (stp.Period.EndDate == null)
                        //        {
                        //            cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            cmd.Parameters.AddWithValue("@endDate", stp.Period.EndDate.Value);
                        //        }

                        //        //cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = stp.Period.EndDate.Value;
                        //        //openCon.Open();

                        //        insertedPeriodId = cmd.ExecuteScalar().ToString();
                        //    }
                        //}

                        #endregion

                        string strSaveStdPrice = "INSERT INTO tblStandardItemPrice (Id, ItemId, PeriodId, StandardPrice) VALUES (@id, @itemId, @periodId, @stdPrice)";
                        using (SqlCommand cmd = new SqlCommand(strSaveStdPrice))
                        {
                            cmd.Connection = openCon;
                            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                            cmd.Parameters.Add("@itemId", SqlDbType.VarChar, 50).Value = insertedItemId;
                            cmd.Parameters.Add("@periodId", SqlDbType.VarChar, 50).Value = insertedPeriodId;
                            cmd.Parameters.Add("@stdPrice", SqlDbType.Decimal).Value = stp.StandardPrice;
                            //openCon.Open();

                            cmd.ExecuteNonQuery();
                        }
                    }

                    #endregion

                    #region For ItemType == DIY

                    #region DIYProfile
                    foreach (DIYProfile stdItmProf in item.lstDIYProfile)
                    {
                        #region Insert into Period
                        //string insertedPeriodId = this.InsertPeriod(stdItmProf.Period);
                        //if (insertedPeriodId == null)
                        //{
                        //    string strSavePeriod = "INSERT INTO tblPeriod (Id, StartDate, EndDate) output INSERTED.Id VALUES (@id, @startDate, @endDate)";
                        //    using (SqlCommand cmd = new SqlCommand(strSavePeriod))
                        //    {
                        //        cmd.Connection = openCon;
                        //        cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                        //        cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = stdItmProf.Period.StartDate;
                        //        if (stdItmProf.Period.EndDate == null)
                        //        {
                        //            cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            cmd.Parameters.AddWithValue("@endDate", stdItmProf.Period.EndDate.Value);
                        //        }
                        //        //cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = stdItmProf.Period.EndDate.Value;
                        //        //openCon.Open();

                        //        insertedPeriodId = cmd.ExecuteScalar().ToString();
                        //    }
                        //}

                        #endregion

                        #region Insert into DIYProfile
                        string diyProfileId = "";
                        string strSaveStdItmProfile = "INSERT INTO tblDIYProfile (Id, ItemId, PeriodId, DIYPriceTypeId, MinChosen, MinProportion) output INSERTED.Id VALUES (@id, @itemId, @periodId, @diyPriceTypeId, @minChosen, @minProportion)";
                        using (SqlCommand cmd = new SqlCommand(strSaveStdItmProfile))
                        {

                            cmd.Connection = openCon;
                            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                            cmd.Parameters.Add("@itemId", SqlDbType.VarChar, 50).Value = insertedItemId;
                            cmd.Parameters.Add("@periodId", SqlDbType.VarChar, 50).Value = insertedPeriodId;
                            cmd.Parameters.Add("@diyPriceTypeId", SqlDbType.VarChar, 50).Value = stdItmProf.PriceType.Id;
                            cmd.Parameters.Add("@minChosen", SqlDbType.Decimal).Value = stdItmProf.MinChosen;
                            cmd.Parameters.Add("@minProportion", SqlDbType.Decimal).Value = stdItmProf.MinProportion;

                            //openCon.Open();

                            diyProfileId = cmd.ExecuteScalar().ToString();
                        }

                        #endregion

                        #region Insert into tblDIYFixedPrice
                        foreach (var tmp in stdItmProf.lstDIYFixedPrice)
                        {
                            #region Insert into Period
                            //string periodId = this.InsertPeriod(tmp.Period);
                            //if (periodId == null)
                            //{
                            //    string strSavePeriod = "INSERT INTO tblPeriod (Id, StartDate, EndDate) output INSERTED.Id VALUES (@id, @startDate, @endDate)";
                            //    using (SqlCommand cmd = new SqlCommand(strSavePeriod))
                            //    {
                            //        cmd.Connection = openCon;
                            //        cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                            //        cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = stdItmProf.Period.StartDate;
                            //        if (stdItmProf.Period.EndDate == null)
                            //        {
                            //            cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
                            //        }
                            //        else
                            //        {
                            //            cmd.Parameters.AddWithValue("@endDate", stdItmProf.Period.EndDate.Value);
                            //        }
                            //        //cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = stdItmProf.Period.EndDate.Value;
                            //        //openCon.Open();

                            //        periodId = cmd.ExecuteScalar().ToString();
                            //    }
                            //}

                            #endregion

                            #region Insert into DIYFixedPrice

                            string strSaveDIYFixedPrice = "INSERT INTO tblDIYFixedPrice (Id, DIYProfileId, PeriodId, Value) VALUES (@id, @diyProfileId, @periodId, @value)";
                            using (SqlCommand cmd = new SqlCommand(strSaveDIYFixedPrice))
                            {

                                cmd.Connection = openCon;
                                cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                cmd.Parameters.Add("@diyProfileId", SqlDbType.VarChar, 50).Value = diyProfileId;
                                cmd.Parameters.Add("@periodId", SqlDbType.VarChar, 50).Value = insertedPeriodId;
                                cmd.Parameters.Add("@value", SqlDbType.Decimal).Value = tmp.Value;


                                //openCon.Open();

                                cmd.ExecuteNonQuery();
                            }

                            #endregion
                        }
                        #endregion

                        #region Insert into tblDIYDefinition

                        foreach (var tmp in stdItmProf.lstDIYDefinitions)
                        {
                            string strSaveDIYDef = "INSERT INTO tblDIYDefinition (Id, DIYProfileId, ItemId) VALUES (@id, @diyProfileId, @itemId)";
                            using (SqlCommand cmd = new SqlCommand(strSaveDIYDef))
                            {

                                cmd.Connection = openCon;
                                cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                cmd.Parameters.Add("@diyProfileId", SqlDbType.VarChar, 50).Value = diyProfileId;
                                cmd.Parameters.Add("@itemId", SqlDbType.VarChar, 50).Value = tmp.Item.Id;

                                //openCon.Open();

                                cmd.ExecuteNonQuery();
                            }
                        }
                        
                        #endregion
                    }
                    #endregion


                   


                    #endregion
                }
            }
            catch (Exception ex) { throw ex; }
            
        }
        public string InsertPeriod(Period period)
        {
            using (SqlConnection openCon = new SqlConnection(this.ConnectionString))
            {
                string strGetPeriod = "SELECT * FROM tblPeriod WHERE StartDate = @startDate AND EndDate = @endDate";
                using (SqlCommand cmd = new SqlCommand(strGetPeriod))
                {
                    cmd.Connection = openCon;
                    cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = period.StartDate;
                    if (period.EndDate == null)
                    {
                        cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@endDate", period.EndDate.Value);
                    }
                    //cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = period.EndDate.Value;
                    openCon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        string id = reader["Id"].ToString();
                        reader.Close();
                        openCon.Close();
                        return id;
                    }
                }
            }

            return null;
        }

        public string InsertDIYTransaction(DIYPrice diyPrice)
        {
            //using (SqlConnection openCon = new SqlConnection(this.ConnectionString))
            //{
            //    string strGetPeriod = "INSERT INTO tblPeriod (Id, StartDate, EndDate) output INSERTED.Id VALUES (@id, @startDate, @endDate)";
            //    using (SqlCommand cmd = new SqlCommand(strGetPeriod))
            //    {
            //        cmd.Connection = openCon;
            //        cmd.Parameters.Add("@startDate", SqlDbType.VarChar, 50).Value = period.StartDate;
            //        cmd.Parameters.Add("@endDate", SqlDbType.VarChar, 50).Value = period.EndDate;
            //        openCon.Open();
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        if (reader.HasRows)
            //        {
            //            string id = reader["Id"].ToString();
            //            reader.Close();
            //            openCon.Close();
            //            return id;
            //        }
            //    }
            //}

            return null;
        }

    }
}

