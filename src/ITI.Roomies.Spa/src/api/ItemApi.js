import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'
import { async } from 'q';

const endpoint = process.env.VUE_APP_BACKEND + "/api/item";

///get the list of items of a specific grocery list
export async function getItemListAsync(courseId) {
  return await getAsync(`${endpoint}/getItemList/${courseId}`);
}

export async function createItemAsync(model) {
  return await postAsync(`${endpoint}/addItem`, model);
}

export async function getItemByItemIdAsync(itemId) {
  return await getAsync(`${endpoint}/getItemByItemId/${itemId}`);
}
// export async function getAllAsync(){
//   return await getAsync(`${endpoint}`);
// }

export async function updateItemAsync(model){
  return await postAsync(`${endpoint}/updateItem`, model);
}

export async function deleteItemAsync(itemId){
  return await deleteAsync(`${endpoint}/delete/${itemId}`);
}