import { getAsync, postAsync, putAsync, deleteAsync } from '../../../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/tdepense";

export async function getAllTransacDepenseAsync(){
  return await getAsync(`${endpoint}/getAllTDepense`);
}

export async function getTransacDepenseAsync(transacDepenseId){
  return await getAsync(`${endpoint}/getTDepense/${transacDepenseId}`);
}

export async function createTransacDepenseAsync(model){
  return await postAsync(`${endpoint}/createTDepense`, model);
}

export async function updateTransacDepenseAsync(model){
  return await putAsync(`${endpoint}/updateTDepense`, model);
}

export async function deleteTDepenseAsync(tDepenseId){
  return await deleteAsync(`${endpoint}/deleteTDepense/${tDepenseId}`)
}
