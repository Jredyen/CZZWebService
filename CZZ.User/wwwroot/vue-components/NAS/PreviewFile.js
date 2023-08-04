import { ref, computed, watch, onMounted } from 'https://unpkg.com/vue@3/dist/vue.esm-browser.js'

export default {
    props: {
        folder: String,
        file: String
    },
    setup(props) {
        const folder = computed(() => props.folder);
        const file = computed(() => props.file);
        const fileExtension = ref('');
        const controller = 'https://localhost:7224/CZZ/GetNASOpenFile?File=NAS';

        watch(file, () => {
            fileExtension.value = file.value.toLowerCase().split('.').pop();
        })

        return {
            folder,
            file,
            fileExtension,
            controller
        }
    },
    template: 
    `
    <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-fullscreen">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">{{file}}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <img v-if="fileExtension === 'png' || fileExtension === 'jpg' || fileExtension === 'jpge'" :src="controller + folder + file" class="img-fluid" />

          </div>
        </div>
      </div>
    </div>
    `
}