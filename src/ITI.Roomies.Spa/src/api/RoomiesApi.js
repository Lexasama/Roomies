import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/Roomies";

export async function getRoomieByIdAsync(roomieId) {
    return await getAsync(`${endpoint}/${roomieId}`);
}

export async function FindByEmail() {
    return await getAsync(endpoint);
}

export async function createRoomieAsync(model) {
    return await postAsync(endpoint, model);
}

export async function inviteRoomieAsync(email){
    return await postAsync((`${endpoint}/${email}/invite`));
}

