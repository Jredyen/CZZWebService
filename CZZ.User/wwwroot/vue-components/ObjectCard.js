import { computed, onBeforeUnmount, onUnmounted, onMounted } from 'https://unpkg.com/vue@3/dist/vue.esm-browser.js'

export default {
    props: {
        item: Object,
        index: Number,
        web: String
    },
    setup(props) {
        console.log('setup: 開始創建組件之前，在 beforeCreate 和 created 之前執行，創建的是 data 和 method');

        const item = computed(() => props.item);
        const index = computed(() => props.index);
        const web = computed(() => props.web);

        onUnmounted(() => {
            console.log('onUnmounted: 組件卸除後執行的函數');
        })
        onMounted(() => {
            console.log('onMounted: 組件完成初始渲染並創建 DOM 元素後執行程式碼');
        })
        onBeforeUnmount(() => {
            console.log('onBeforeUnmount: 組件卸除前執行的函數');
        })

        return {
            item,
            index,
            web
        }
    },
    template: `
                <div class="col">                 
                    <div class="card">
                        <div :id="'carousel' + index" data-bs-ride="carousel" class="carousel slide" v-carousel>

                            <div class="carousel-indicators">
                                <template v-for="(image,image_index) in item.photo_list">
                                    <button type="button" :data-bs-target="'#carousel' + index" :data-bs-slide-to="image_index" :class="{'active':image_index === 0}" aria-current="true" :aria-label="'Slide' + image_index"></button>
                                </template>
                            </div>

                            <div v-if="item.photo_list.length > 0">
                                <div class="carousel-inner">
                                    <div v-for="(image,image_index) in item.photo_list" class="carousel-item" :class="{'active':image_index === 0}">
                                        <img :src="image" class="d-block img-fluid">
                                    </div>
                                </div>
                                <button class="carousel-control-prev" type="button" :data-bs-target="'#carousel' + index" data-bs-slide="prev">
                                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Previous</span>
                                </button>
                                <button class="carousel-control-next" type="button" :data-bs-target="'#carousel' + index" data-bs-slide="next">
                                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Next</span>
                                </button>
                            </div>

                            <div v-else>
                                <img src="https://s.591.com.tw/build/static/house/rentDetail/images/no-img.png" class="d-block img-fluid">
                            </div>
                        </div>

                        <div class="card-body d-flex flex-column align-items-center">
                            <h5 v-html="item.title" class="card-title"></h5>
                            <p>{{item.area}} 坪   {{item.price}} {{item.price_unit}}  {{item.role_name}}</p>
                            <p>{{item.floor_str}}   {{item.kind_name}}   {{item.room_str}}</p>
                            <p v-html="item.location"></p>
                            <a :href="web + item.post_id" target="_blank" class="btn btn-primary">去看看</a>
                        </div>
                    </div>
                </div>
                `
}