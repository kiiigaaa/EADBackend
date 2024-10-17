// File Name : ApiConstant.cs
// Date : 08/10/2024
// This class contains constant values used throughout the WebApi application. It includes response status strings and messages for various API operations related to products, categories, roles, users, cart items, and orders. These constants are used to ensure consistency and avoid magic strings in the code.

namespace WebApi.Constant
{
    public class ApiConstant
    {
        #region Response Status
        public const string Success = "Success";
        public const string Error = "Error";
        #endregion

        #region Response Messages

        public const string MsgInternalServerError = "server-Error.";

        public const string MsgProductDoesNotExist = "product-not-exist";

        public const string MsgProductsRetrievalSuccess = "products-retrieval-success";
        public const string MsgProductRetrievalSuccess = "product-retrieval-success";
        public const string MsgProductCreationSuccess = "product-creation-success";

        public const string MsgProductUpdateSuccess = "product-update-success";
        public const string MsgProductUpdateFailed = "product-update-failed";

        public const string MsgProductRemoveSuccess = "product-remove-success";
        public const string MsgProductRemoveFailed = "product-remove-failed";

        public const string MsgCategoryDoesNotExist = "Category-not-exist";

        public const string MsgCategoriesRetrievalSuccess = "Categories-retrieval-success";
        public const string MsgCategoryRetrievalSuccess = "Category-retrieval-success";
        public const string MsgCategoryCreationSuccess = "Category-creation-success";

        public const string MsgCategoryUpdateSuccess = "Category-update-success";
        public const string MsgCategoryUpdateFailed = "Category-update-failed";

        public const string MsgCategoryStatusUpdateSuccess = "Category-status-update-success";
        public const string MsgCategoryStatusUpdateFailed = "Category-status-update-failed";

        public const string MsgRolesRetrievalSuccess = "Roles-retrieval-success";
        public const string MsgRoleCreationSuccess = "Role-creation-success";

        public const string MsgRoleAlreadyExist = "Role-already-exist";

        public const string MsgUserAlreadyExist = "User-already-exist";
        public const string MsgUserNotExist = "User-not-exist";

        public const string MsgUsersByroleRetrievalSuccess = "Users-by-role-retrieval-success";
        public const string MsgUsersByroleDoesNotExist = "Users-by-this-role-not-found";

        public const string MsgUserCreationSuccess = "User-creation-success";
        public const string MsgUserCreationFailed = "User-creation-failed";

        public const string MsgUserUpdateSuccess = "User-update-success";
        public const string MsgUserUpdateFailed = "User-update-failed";

        public const string MsgUserLoginSuccess = "User-login-success";

        public const string MsgUserDetailsRetrievalSuccess = "User-details-retrieval-success";

        public const string MsgCartItemsRetrievalSuccess = "Cart-items-retrieval-success";
        public const string MsgCartItemsNotFound = "Cart-items-not-found";
        public const string MsgCartItemRemovalSuccess = "Cart-item-removal-success";
        public const string MsgAddItemToCartSuccess = "add-item-to-cart-success";

        public const string MsgStatusCreationSuccess = "Status-creation-success";
        public const string MsgStatusRetrievalSuccess = "Status-retrieval-success";
        public const string MsgStatusDoesNotExist = "Statuses-not-found";

        public const string MsgOrderCreationSuccess = "Order-creation-success";
        public const string MsgOrderCreationFailed = "Order-creation-failed";
        public const string MsgOrderRetrievalSuccess = "Order-retrieval-success";
        #endregion

    }
}
