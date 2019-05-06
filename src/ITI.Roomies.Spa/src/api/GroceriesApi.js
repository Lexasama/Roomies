import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/Groceries";


export async function getGroceryListByIdAsync(courseId) {
  return await getAsync(`${endpoint}/${courseId}`);
}

export async function createGroceryListAsync(model) {
  return await postAsync(endpoint, model);
}

export async function getAllAsync() {
  return await getAsync(`${endpoint}`);
}

export async function updateAgroceryListAsync( model ) {
  return await putAsync(`${endpoint}/${model.courseId}`, model);
}

export async function deleteAGroceryListAsync(courseId){
  return await deleteAsync(`${endpoint}/${courseId}`);
}