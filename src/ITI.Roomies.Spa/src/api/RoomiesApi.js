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

export async function uploadRoomieImageAsync(roomieId,formData) {

    
    var request = new XMLHttpRequest();
    
    request.open("POST", "{id}/upload/");
    console.log(request);
    debugger;
    
    image = request.send(formData);
    console.log(image);
    debugger;

    return await postAsync((`${endpoint}/${roomieId}/upload/${image}`));

    
}