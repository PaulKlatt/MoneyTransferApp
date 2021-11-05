﻿using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TenmoClient.Exceptions;
using TenmoClient.Models;
using TenmoServer.Models;

namespace TenmoClient
{
    public class ApiService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly static string ACCOUNTS_URL = API_BASE_URL + "accounts";
        private readonly static string USERS_URL = API_BASE_URL + "users";
        private readonly static string TRANSACTIONS_URL = API_BASE_URL + "transactions";
        private readonly IRestClient client = new RestClient();
        //private static ApiUser user = new ApiUser();

        public List<Account> ViewAccountsByUserId(int userId)
        {
            RestRequest request = new RestRequest($"{ACCOUNTS_URL}/{userId}");
            IRestResponse<List<Account>> response = client.Get<List<Account>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            return response.Data;
        }

        public List<UserInfo> GetAllUsers()
        {
            RestRequest request = new RestRequest(USERS_URL);
            IRestResponse<List<UserInfo>> response = client.Get<List<UserInfo>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            return response.Data;
        }
        //Account accountFrom, Account accountTo, decimal amount
        public Transfer SendAmount(Transfer transfer)
        {
            transfer.TransferStatusDesc = "Approve";
            transfer.TransferTypeDesc = "Send";
            RestRequest request = new RestRequest();
            IRestResponse<Transfer> response = client.Post<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            return response.Data;
        }

        public void ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new NoResponseException("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("Authorization is required for this endpoint. Please log in");
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new ForbiddenException("You do not have permission to perform the requested action.");
                }
                else
                {
                    throw new NonSuccessException((int)response.StatusCode);
                }
            }
        }
    }
}