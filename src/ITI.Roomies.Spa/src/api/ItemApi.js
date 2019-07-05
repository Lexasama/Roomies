import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/item";

///get the list of items of a specific grocery list
export async function getItemListAsync(courseId) {
  return await getAsync(`${endpoint}/getItemList/${courseId}`);
}


export async function getSavedItemListFromCollocAsync(collocId) {
  return await getAsync(`${endpoint}/getAllSavedItemList/${collocId}`);
}

// export async function getRItemListAsync(courseTempId) {
//   return await getAsync(`${endpoint}/getRItems/${courseTempId}`);
// }

export async function createItem(model) {
  return await postAsync(`${endpoint}/createItem`, model);
}

export async function createItemInListAsync(itemId, courseId, roomieId, itemQuantite) {
  return await postAsync(`${endpoint}/createItemInList/${itemId}/${courseId}/${roomieId}/${itemQuantite}`);
}

export async function getItemByItemIdAsync(itemId) {
  return await getAsync(`${endpoint}/${itemId}`);
}
//  export async function getRItemByIdAsync(rItemId) {
//    return await getAsync(`${endpoint}/getRItem/${rItemId}`);
//  }

export async function updateItemAsync(model){
  return await putAsync(`${endpoint}/updateItem`, model);
}

// export async function updateRItemAsync(model) {
//   return await putAsync5(`${endpoint}/updateRItem`, model)
// }

export async function deleteItemAsync(itemId){
  return await deleteAsync(`${endpoint}/${itemId}`);
}

export async function deleteSavedItemAsync(itemId){
  return await deleteAsync(`${endpoint}/deleteSavedItem/${itemId}`);
}

export async function deleteItemFromListAsync(itemId, courseId, roomieId, itemSaved){
  return await deleteAsync(`${endpoint}/deleteFromCourse/${itemId}/${courseId}/${roomieId}/${itemSaved}`);
}

// export async function deleteRItemAsync(rItemId) {
//   return await deleteAsync(`${endpoint}/deleteRItem/${rItemId}`);
// }

export async function UpdateItemBoughtAsync(itemId, bought) {
  return await postAsync(`${endpoint}/updateItemBought/${itemId}/${bought}`);
}