using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;

namespace FinanceManagement.Infrastructure.Services
{
    public class CategoryServicecs : ICategoryService
    {
        private readonly ICategoryRepository _category;

        public CategoryServicecs(ICategoryRepository _category)
        {
            this._category = _category;
        }
        public async Task<ApiResponse<CategoryDto>> CreateAsync(CategoryDto category)
        {
            var response = new ApiResponse<CategoryDto>();

            try
            {
                var newCategory = new Category
                {
                    CategoryName = category.CategoryName,
                    IsRecurring = category.IsRecurring
                };
                var data = await _category.CreateAsync(newCategory);

                var createdCategory = new CategoryDto
                {
                    //Id = data.Id,
                    CategoryName = data.CategoryName,
                    IsRecurring = data.IsRecurring
                };
                response.Success = true;
                response.Message = "Category created successfully.";
                response.Data = createdCategory;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "data not insert";
            }

            return response;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _category.DeleteAsync(id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApiResponse<IEnumerable<Category>>> GetAllAsync()
        {
            var response = new ApiResponse<IEnumerable<Category>>();

            try
            {
                var data = await _category.GetAllAsync();

                response.Success = true;
                response.Message = "Category list fetched successfully.";
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Sorry no data found";
            }

            return response;
        }

        public async Task<ApiResponse<Category?>> GetByIdAsync(int id)
        {
            var response = new ApiResponse<Category?>();

            try
            {
                var data = await _category.GetByIdAsync(id);

                if (data == null)
                {
                    response.Success = false;
                    response.Message = "Category not found.";
                }
                else
                {
                    response.Success = true;
                    response.Message = "Category fetched successfully.";
                    response.Data = data;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public async Task<ApiResponse<CategoryDto>> UpdateAsync(int id, CategoryDto category)
        {
            var response = new ApiResponse<CategoryDto>();

            try

            {
                var newCategory = new Category
                {
                    CategoryName = category.CategoryName,
                    IsRecurring = category.IsRecurring
                };
                var data = await _category.UpdateAsync(id, newCategory);
                var createdCategory = new CategoryDto
                {
                    //Id = data.Id,
                    CategoryName = data.CategoryName,
                    IsRecurring = data.IsRecurring
                };
                response.Success = true;
                response.Message = "Category updated successfully.";
                response.Data = createdCategory;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

    }
}
