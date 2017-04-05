// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Linq;
using Microsoft.Azure;
using Microsoft.Azure.Management.Sql.LegacySdk.Models;

namespace Microsoft.Azure.Management.Sql.LegacySdk.Models
{
    /// <summary>
    /// Response for long running Azure SQL Database Failover Group delete
    /// operations.
    /// </summary>
    public partial class FailoverGroupDeleteResponse : AzureOperationResponse
    {
        private ErrorResponse _error;
        
        /// <summary>
        /// Optional. Gets or sets the error details if available.
        /// </summary>
        public ErrorResponse Error
        {
            get { return this._error; }
            set { this._error = value; }
        }
        
        private string _operationStatusLink;
        
        /// <summary>
        /// Optional. Gets or sets the location header value.
        /// </summary>
        public string OperationStatusLink
        {
            get { return this._operationStatusLink; }
            set { this._operationStatusLink = value; }
        }
        
        private int _retryAfter;
        
        /// <summary>
        /// Optional. Gets or sets how long to wait before polling.
        /// </summary>
        public int RetryAfter
        {
            get { return this._retryAfter; }
            set { this._retryAfter = value; }
        }
        
        private OperationStatus _status;
        
        /// <summary>
        /// Optional. Gets or sets the status of the operation.
        /// </summary>
        public OperationStatus Status
        {
            get { return this._status; }
            set { this._status = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the FailoverGroupDeleteResponse class.
        /// </summary>
        public FailoverGroupDeleteResponse()
        {
        }
    }
}
