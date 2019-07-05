import { getAsync, postAsync, putAsync, deleteAsync } from '../../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/category";

export async  function getCategoryAsync(categoryId){
  return await getAsync(`${endpoint}/getCategory/${categoryId}`);
}

export async function getCategoriesAsync(collocId){
  return await getAsync(`${endpoint}/getCategories/${collocId}`);
}

export async function createCategoryAsync(model){
  return await postAsync(`${endpoint}/create`, model)
}

export async function updateCategoryAsync(model){
  return await putAsync(`${endpoint}/update`, model);
}

export async function deleteCategoryAsync(categoryId){
  return await deleteAsync(`${endpoint}/${categoryId}`);
}

export async function getDefaultIconsAsync(){
  return await getAsync(`${endpoint}/getIcons`);
}
