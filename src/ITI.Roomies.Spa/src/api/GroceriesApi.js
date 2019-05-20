import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/course";


export async function getGroceryListByIdAsync(courseId) {
  return await getAsync(`${endpoint}/GetAGroceryList/${courseId}`);
}

export async function createGroceryListAsync(model) {
  return await postAsync(`${endpoint}/createGroceryList`, model);
}


export async function getAllAsync(collocId) {
  return await getAsync(`${endpoint}/getList/${collocId}`);
}

export async function updateAgroceryListAsync( model ) {
  return await putAsync(`${endpoint}/updateGroceryList`, model);
}

export async function deleteAGroceryListAsync(courseId){
  return await deleteAsync(`${endpoint}/${courseId}`);
}