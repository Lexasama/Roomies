import { getAsync, postAsync, putAsync, deleteAsync } from '../../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/category";

export async  function getCategoryAsync(categoryId){
  return await getAsync(`${endpoint}/GetCategory/${categoryId}`);
}

export async function getCategoriesAsync(collocId){
  return await getAsync(`${endpoint}/GetCategories/${collocId}`);
}

export async function createCategoryAsync(model){
  return await postAsync(`${endpoint}/create`, model)
}

export async function updateCategoryAsync(categoryId){
  return await putAsync(`${endpoint}/${categoryId}`);
}

export async function deleteCategoryAsync(categoryId){
  return await deleteAsync(`${endpoint}/${categoryId}`);
}

export async function getDefaultIconsAsync(){
  return await getAsync(`${endpoint}/getIcons`);
}