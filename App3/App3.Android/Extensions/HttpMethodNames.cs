﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Extensions
{
    public static class HttpMethodNames
    {
        public static string Head
        {
            get
            {
                return "HEAD";
            }
        }

        public static string Post
        {
            get
            {
                return "POST";
            }
        }

        public static string Put
        {
            get
            {
                return "PUT";
            }
        }

        public static string Get
        {
            get
            {
                return "GET";
            }
        }

        public static string Delete
        {
            get
            {
                return "DELETE";
            }
        }

        public static string Trace
        {
            get
            {
                return "TRACE";
            }
        }

        public static string Options
        {
            get
            {
                return "OPTIONS";
            }
        }

        public static string Connect
        {
            get
            {
                return "CONNECT";
            }
        }

        public static string Patch
        {
            get
            {
                return "PATCH";
            }
        }
    }
}
