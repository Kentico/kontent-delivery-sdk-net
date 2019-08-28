﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace KenticoCloud.Delivery
{
    /// <summary>
    /// Represents a response from Kentico Cloud Delivery API that contains a list of taxonomy groups.
    /// </summary>
    public sealed class DeliveryTaxonomyListingResponse : AbstractResponse
    {
        /// <summary>
        /// Gets paging information.
        /// </summary>
        public Pagination Pagination { get; }

        /// <summary>
        /// Gets a list of taxonomy groups.
        /// </summary>
        public IReadOnlyList<TaxonomyGroup> Taxonomies { get; }

        /// <summary>
        /// Gets a value that determines if content is stale.
        /// Stale content indicates that there is a more recent version, but it will become available later.
        /// Stale content should be cached only for a limited period of time.
        /// </summary>
        public bool HasStaleContent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryTaxonomyListingResponse"/> class with information from a response.
        /// </summary>
        /// <param name="response">A response from Kentico Cloud Delivery API that contains a list of taxonomy groups.</param>
        /// /// <param name="apiUrl">API URL used to communicate with the underlying Kentico Cloud endpoint.</param>
        internal DeliveryTaxonomyListingResponse(ApiResponse response, string apiUrl) : base(apiUrl)
        {
            Pagination = response.Content["pagination"].ToObject<Pagination>();
            Taxonomies = ((JArray)response.Content["taxonomies"]).Select(type => new TaxonomyGroup(type)).ToList().AsReadOnly();
            HasStaleContent = response.HasStaleContent;
        }
    }
}
