// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace SoftinuxBase.WebApplication
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestResponseLoggingMiddleware(RequestDelegate next_, ILoggerFactory loggerFactory_)
        {
            _next = next_;
            _logger = loggerFactory_
                      .CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context_)
        {
            await LogRequest(context_);
            await LogResponse(context_);
        }

        private static string ReadStreamInChunks(Stream stream_)
        {
            const int readChunkBufferLength = 4096;
            stream_.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream_);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            }
            while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context_)
        {
            // TODO Need code this logger.
        }

        private async Task LogRequest(HttpContext context_)
        {
            context_.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context_.Request.Body.CopyToAsync(requestStream);
            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                   $"Schema:{context_.Request.Scheme} " +
                                   $"Host: {context_.Request.Host} " +
                                   $"Path: {context_.Request.Path} " +
                                   $"QueryString: {context_.Request.QueryString} " +
                                   $"Request Body: {ReadStreamInChunks(requestStream)}");
            context_.Request.Body.Position = 0;
        }
    }
}