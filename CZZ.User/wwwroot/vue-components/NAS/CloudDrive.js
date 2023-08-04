import { ref, computed, watch, onMounted } from 'https://unpkg.com/vue@3/dist/vue.esm-browser.js'
import PreviewFile from '/vue-components/NAS/PreviewFile.js';

export default {
    components: {
        'preview-file': PreviewFile
    },
    setup() {
        const folderUrl = "https://localhost:7224/CZZ/GetNASFolderPath";
        const filesUrl = "https://localhost:7224/CZZ/GetNASFilesPath";
        const FoldersPath = ref([]);
        const FilesPath = ref([]);
        const SelectedFolder = ref('');
        const NowFolderPaths = ref([]);
        const NowFolderPath = ref('');
        const SelectedFile = ref('');

        onMounted(() => {
            fetchPath();
        })

        function fetchPath(folder = '') {
            axios.get(folderUrl + folder)
                .then((response) => {
                    FoldersPath.value = response.data.folders;
                    NowFolderPath.value = response.data.folderPaht;

                    const spiltNowFolderPath = [...JSON.parse(JSON.stringify(NowFolderPath.value)).split("\\").filter(t => t != '')];

                    NowFolderPaths.value = [];

                    for (let i = 0; i < spiltNowFolderPath.length; i++) {
                        const url = spiltNowFolderPath.slice(0, i + 1).join("\\");
                        NowFolderPaths.value.push({
                            title: spiltNowFolderPath[i],
                            url: '\\' + url
                        });
                    }
                })
                .catch((error) => {
                    console.error("發生錯誤：", error);
                });

            axios.get(filesUrl + folder)
                .then((response) => {
                    FilesPath.value = response.data;
                })
                .catch((error) => {
                    console.error("發生錯誤：", error);
                });
        }

        watch(SelectedFolder, () => {
            SelectedFile.value = '';
            fetchPath('?folder=' + SelectedFolder.value);
        })

        return {
            folderUrl,
            FoldersPath,
            FilesPath,
            SelectedFolder,
            NowFolderPaths,
            NowFolderPath,
            SelectedFile
        }
    },
    template: 
    `
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        
        <li class="breadcrumb-item"><a href="#" v-on:click="SelectedFolder = ''">Cloud Drive</a></li>
        <li class="breadcrumb-item" v-for="path in NowFolderPaths" ><a href="#" v-on:click="SelectedFolder = path.url">{{path.title}}</a></li>
      </ol>
    </nav>


    <div>
        資料夾
        <p v-for="folder in FoldersPath"><a href="#" v-on:click="SelectedFolder = NowFolderPath + folder ">{{folder}}</a></p>
    </div>
    <hr>
    <div>
        檔案
        <p v-for="file in FilesPath"><a href="#" v-on:click="SelectedFile = file" data-bs-toggle="modal" data-bs-target="#exampleModal">{{file}}</a></p>
    </div>

    <div>
        <preview-file :file="SelectedFile" :folder="NowFolderPath"></preview-file>
    </div>
    `
}