import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/course";


export async function getGroceryListByIdAsync(courseId) {
  return await getAsync(`${endpoint}/GetList/${courseId}`);
}
export async function getTemplateById( courseId){
  return await getAsync(`${endpoint}/GetTemplates/${courseId}`);
}
export async function createTemplateOrListAsync( model ) {
  return await postAsync(`${endpoint}/create`, model)
}

export async function getAllListsAsync(collocId) {
  return await getAsync(`${endpoint}/getLists/${collocId}`);
}

export async function getAllTemplatesAsync( collocId) {
  return await getAsync(`${endpoint}/getTemplates/${collocId}`);
}

export async function updateListOrTemplateAsync( model ) {
  return await putAsync(`${endpoint}/update`, model);
}

export async function deleteListAsync(courseId){
  return await deleteAsync(`${endpoint}/${courseId}`);
}

export async function deleteTemplateAsync( courseId ) {
  return await deleteAsync(`${endpoint}/deleteTemplate/${courseId}`);
}
