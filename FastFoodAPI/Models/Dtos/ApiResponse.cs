﻿using System.Net;

namespace FastFoodHouse_API.Models.Dtos
{
    public class ApiResponse
    {
        //public ApiResponse()
        //{
        //    ErrorMessages = new List<string>();
        //}
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
    }
}
