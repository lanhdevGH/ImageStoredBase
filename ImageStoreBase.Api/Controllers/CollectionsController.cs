﻿using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.Filters;
using ImageStoreBase.Api.FluentValidator;
using ImageStoreBase.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageStoreBase.Api.Controllers
{
    public class CollectionsController : BaseController
    {
        private readonly ICollectionService _collectionService;

        public CollectionsController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var pagedResult = await _collectionService.GetPagedAsync(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var collections = await _collectionService.GetAllAsync();
            return Ok(collections);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var collection = await _collectionService.GetByIdAsync(id);
            if (collection == null) return NotFound();
            return Ok(collection);
        }

        [HttpPost]
        [FluentValidationEntityFilter<CollectionCreateRequestDTO, CollectionCreateRequestDTOValidator>("entity")]
        public async Task<IActionResult> Create([FromBody] CollectionCreateRequestDTO entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _collectionService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, entity);
        }

        [HttpPut("{id}")]
        [ValidateEntityExistsFilter<Collection>("id")]
        public async Task<IActionResult> Update(string id, [FromBody] CollectionUpdateRequestDTO collection)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _collectionService.UpdateAsync(id, collection);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _collectionService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
