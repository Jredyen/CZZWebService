const apiUrl = "https://localhost:7224/CZZ/GetPath";

import { ref, computed, onMounted } from 'https://unpkg.com/vue@3/dist/vue.esm-browser.js'
import ObjectList from '/vue-components/House/components/ObjectList.js'

export default {
    components: {
        'object-list': ObjectList
    },
    methods: {
        backToList() {
            this.date = '';
        }
    },
    setup() {
        const items = ref([]);
        const error = ref('');
        const date = ref('');

        onMounted(async () => {
            await fetchData();
        })

        async function fetchData() {
            axios.get(apiUrl)
                .then((response) => {
                    items.value = response.data;
                })
                .catch((error) => {
                    error.value = error;
                });
        }

        const fItems = computed(() => {
            const datas = [...items.value.filter(t => {
                return t !== 'database';
            })]
            return datas
        });

        return {
            fItems,
            error,
            date
        }
    },
    template:
    `
    <ul :style="{ display: date ? 'none' : 'block' }">
        <li v-for="item in fItems" v-on:click="date = item">{{item}}</li>
    </ul>
    <div v-if="date">
        <object-list :date="date" @backToList="backToList"></object-list>
    </div>
    `
}