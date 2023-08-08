import { ref, computed, watch } from 'https://unpkg.com/vue@3/dist/vue.esm-browser.js'

export default {
    props: ['count'],
    methods: {
        changeKeyword() {
            this.$emit('keyword', keyword)
        }
    },
    setup(props) {
        const keyword = ref('');
        const count = computed(() => props.count)

        watch(keyword, () => {
            changeKeyword();
        })

        return {
            keyword,
            count
        }
    },
    template: 
    `
    <div class="input-group mb-3">
      <span v-if="count === 0" class="input-group-text" id="basic-addon1">沒有資料</span>
      <span v-else class="input-group-text" id="basic-addon1">找到 {{count}} 筆資料</span>
      <input v-model="keyword" class="form-control" placeholder = "輸入關鍵字搜尋" aria-label="Username" aria-describedby="basic-addon1">
    </div>
    `
}