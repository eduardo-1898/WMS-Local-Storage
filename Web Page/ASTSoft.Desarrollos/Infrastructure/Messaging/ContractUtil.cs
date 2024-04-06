using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class ContractUtil
    {

        #region Request 

        public static ContractRequest<TRequest> CreateRequest<TRequest>(TRequest dtoData, ContractRequest<TRequest> request)
            where TRequest : class
        {
            var toReturn = new ContractRequest<TRequest>
            {
                Data = dtoData,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return toReturn;
        }

        public static ContractRequest<BaseRequest> CreateBaseRequest<TRequest>(ContractRequest<TRequest> request)
            where TRequest : class
        {
            var toReturn = new ContractRequest<BaseRequest>()
            {
                Data = new BaseRequest(),
                Page = request.Page,
                PageSize = request.PageSize
            };

            return toReturn;
        }

        public static ContractRequest<TRequest> CreateRequest<TRequest>(TRequest dtoData, string culture = null)
            where TRequest : class
        {
            var toReturn = new ContractRequest<TRequest>
            {
                Data = dtoData,
            };

            return toReturn;
        }

        public static ContractRequest<BaseRequest> CreateBaseRequest(string culture = null)
        {
            var toReturn = new ContractRequest<BaseRequest>()
            {
                Data = new BaseRequest(),
            };

            return toReturn;
        }

        #endregion

        #region Response

        public static ContractResponse<TResponse> CreateResponse<TResponse>(TResponse data)
            where TResponse : class
        {

            return new ContractResponse<TResponse>
            {
                Data = data,
                ErrorMessages = new[] { "" },
                IsValid = true,


            };
        }

        #endregion

        #region InvalidReponse

        public static ContractResponse<TResponse> CreateInvalidResponse<TResponse>(TResponse data, string errorMessage)
            where TResponse : class
        {

            return new ContractResponse<TResponse>
            {
                Data = data,
                ErrorMessages = new[] { errorMessage },
                IsValid = false,
            };
        }

        public static ContractResponse<TResponse> CreateInvalidResponse<TResponse>(Exception ex, TResponse data)
            where TResponse : class
        {

            return new ContractResponse<TResponse>
            {
                Data = data,
                ErrorMessages = new[] { ex.Message },
                IsValid = false,
            };
        }

        public static ContractResponse<TResponse> CreateInvalidResponse<TResponse>(Exception ex)
            where TResponse : class
        {

            return new ContractResponse<TResponse>
            {
                Data = null,
                ErrorMessages = new[] { ex.Message },
                IsValid = false,
            };
        }

        public static ContractResponse<TResponse> CreateInvalidResponse<TResponse>(IEnumerable<BusinessRule> brokenRules)
            where TResponse : class
        {

            return new ContractResponse<TResponse>
            {
                Data = null,
                ErrorMessages = brokenRules.Select(b => b.Rule).ToArray(),
                IsValid = false,
            };
        }

        public static ContractResponse<TResponse> CreateInvalidResponse<TOldResponse, TResponse>(ContractResponse<TOldResponse> response)
            where TResponse : class
            where TOldResponse : class
        {

            return new ContractResponse<TResponse>
            {
                Data = null,
                ErrorMessages = response.ErrorMessages,
                IsValid = false,
            };
        }


        #endregion

    }
}
