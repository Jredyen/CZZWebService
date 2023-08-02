import {ref, computed, watch, onMounted} from 'https://unpkg.com/vue@3/dist/vue.esm-browser.js'
import ObjectCard from '/vue-components/House/ObjectCard.js'

const apiUrl = "https://localhost:7224/CZZ/GetObject?Date=";

export default {
    props: {
        date: String
    },
    components: {
        'object-card': ObjectCard
    },
    setup(props) {
        const items = ref([]);
        const filteredCount = ref(0);
        const web = 'https://rent.591.com.tw/home/';
        const queryString = ref('');
        const sortType = ref('');
        const isReverse = ref(false);
        const error = ref('');
        const page = ref(0);
        const filteredPage = ref(0);
        const pageDataCount = ref(10);
        const isCardMode = ref(false);
        const rowCount = ref(3);
        const date = computed(() => props.date)

        watch(date, () => {
            fetchData();
        })

        function fetchData() {
            axios.get(apiUrl + date.value)
                .then((response) => {
                    items.value = response.data.data.data;
                })
                .catch((error) => {
                    error.value = error;
                })
        }

        function changeisCardMode() {
            isCardMode.value = !isCardMode.value
        }

        function changeType(type) {
            console.log(type)
            if (type == sortType.value) {
                isReverse.value = !isReverse.value
            }
            else {
                sortType.value = type
                isReverse.value = false
            }
        }

        watch([pageDataCount, isReverse, sortType, queryString], () => {
            page.value = 0
        })

        const filteredItems = computed(() => {
            const cloned = JSON.parse(JSON.stringify(items.value));
            let temp = [...cloned]
            const searchText = queryString.value
            const type = sortType.value

            if (searchText) {
                temp = [...cloned.filter(t => {
                    return t.title.indexOf(searchText) > -1 || t.location.indexOf(searchText) > -1;
                })]

                temp.forEach(item => {
                    item.location = item.location.replace(searchText, `<span style="color:red;font-weight:bold">${searchText}</span>`)
                    item.title = item.title.replace(searchText, `<span style="color:red;font-weight:bold">${searchText}</span>`)
                })
            }

            temp.sort((a, b) => {
                if (typeof (a[type]) === 'string') {
                    if (isReverse.value) {
                        return (parseInt(b[type].replace(/,/g, ''))) - (parseInt(a[type].replace(/,/g, '')))
                    }
                    else {
                        return (parseInt(a[type].replace(/,/g, ''))) - (parseInt(b[type].replace(/,/g, '')))
                    }
                }
                else {
                    if (isReverse.value) {
                        return b[type] - a[type]
                    }
                    else {
                        return a[type] - b[type]
                    }
                }
            })

            filteredPage.value = Math.ceil(temp.length / pageDataCount.value);
            filteredCount.value = temp.length;
            temp = temp.slice(page.value * pageDataCount.value, pageDataCount.value * page.value + pageDataCount.value)
            return temp;
        })

        const returnColCss = computed(() => ({
            'row-cols-md-2': rowCount.value === 2,
            'row-cols-md-3': rowCount.value === 3,
            'row-cols-md-4': rowCount.value === 4,
        }))

        return {
            items,
            filteredCount,
            web,
            queryString,
            sortType,
            isReverse,
            error,
            page,
            filteredPage,
            pageDataCount,
            filteredItems,
            isCardMode,
            changeisCardMode,
            changeType,
            rowCount,
            returnColCss,
            date
        }
    },
    template: `
<div v-if="!error">

        <h2>{{date}} 的新物件</h2>

        <div class="form-check form-switch">
            <input class="form-check-input" v-on:click="changeisCardMode()" type="checkbox" role="switch" id="flexSwitchCheckDefault">
            <label class="form-check-label" for="flexSwitchCheckDefault">卡片模式</label>
        </div>

        <p>Search <input v-model="queryString" /></p>
        <div>總共 {{filteredCount}} 筆資料</div>

        <nav aria-label="Page navigation example">
            <ul class="pagination">
                <li class="page-item" :class="{'active' : pageDataCount === 10}"><a v-on:click="pageDataCount = 10" class="page-link" tabindex="-1" aria-disabled="true">10</a></li>
                <li class="page-item" :class="{'active' : pageDataCount === 50, 'disabled' : filteredCount < 11}"><a v-on:click="pageDataCount = 50" class="page-link" tabindex="-1" aria-disabled="true">50</a></li>
                <li class="page-item" :class="{'active' : pageDataCount === 100, 'disabled' : filteredCount < 51}"><a v-on:click="pageDataCount = 100" class="page-link" tabindex="-1" aria-disabled="true">100</a></li>
            </ul>
        </nav>

        <nav aria-label="Page navigation example">
            <ul class="pagination justify-content-center">
                <li class="page-item" :class="{'disabled':page === 0}"><a v-on:click="page = 0" class="page-link" href="#" tabindex="-1" aria-disabled="true">&laquo;</a></li>
                <li class="page-item" :class="{'disabled':page === 0}"><a v-on:click="page--" class="page-link" href="#" tabindex="-1" aria-disabled="true">&lt;</a></li>
                <li v-for="index in filteredPage" class="page-item" :class="{'active':page === index - 1}"><a v-on:click="page = index - 1" class="page-link" :name="index" href="#">{{index}}</a></li>
                <li class="page-item" :class="{'disabled':page === filteredPage - 1}"><a v-on:click="page++" class="page-link" href="#">&gt;</a></li>
                <li class="page-item" :class="{'disabled':page === filteredPage - 1}"><a v-on:click="page = filteredPage - 1" class="page-link" href="#">&raquo;</a></li>
            </ul>
        </nav>

        <div v-if="!isCardMode">
            <table class="table">
                <thead>
                    <tr>
                        <th>預覽</th>
                        <th>標題</th>
                        <th class="click" v-on:click="changeType('area')">
                            大小
                            <span class="icon" :class="{'inverse': isReverse}" v-if="sortType == 'area'">
                                <i v-if="!isReverse" class="text-success">▲</i>
                                <i v-else="!isReverse" class="text-danger">▲</i>
                            </span>
                        </th>
                        <th>樓層</th>
                        <th>類型</th>
                        <th class="click" v-on:click="changeType('price')">
                            租金
                            <span class="icon" :class="{'inverse': isReverse}" v-if="sortType == 'price'">
                                <i v-if="!isReverse" class="text-success">▲</i>
                                <i v-else="!isReverse" class="text-danger">▲</i>
                            </span>
                        </th>
                        <th>地址</th>
                        <th>來源</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(item,index) in filteredItems" :key="item.post_id">
                        <td style="width: 20%">
                            <div :id="'carousel'+index" data-bs-ride="carousel" class="carousel slide" v-carousel>
                                <div class="carousel-indicators">
                                    <template v-for="(img,index2) in item.photo_list" >
                                        <button type="button" :data-bs-target="'#carousel'+index" :data-bs-slide-to="index2" :class="{'active':index2===0}" aria-current="true" :aria-label="'Slide '+index2"></button>
                                    </template>
                                </div>
                                <div v-if="item.photo_list.length > 0">
                                    <div class="carousel-inner">
                                        <div v-for="(img,index2) in item.photo_list" class="carousel-item" :class="{'active':index2===0}">
                                            <img :src="img" class="d-block img-fluid">
                                        </div>
                                    </div>
                                    <button class="carousel-control-prev" type="button" :data-bs-target="'#carousel'+index" data-bs-slide="prev">
                                        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Previous</span>
                                    </button>
                                    <button class="carousel-control-next" type="button" :data-bs-target="'#carousel'+index" data-bs-slide="next">
                                        <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                        <span class="visually-hidden">Next</span>
                                    </button>
                                </div>
                                <div v-else>
                                    <img src="https://s.591.com.tw/build/static/house/rentDetail/images/no-img.png" class="d-block img-fluid">
                                </div>
                            </div>
                        </td>
                        <td>
                            <span class="d-inline-block" tabindex="0" data-bs-toggle="popover" data-bs-trigger="hover focus" data-bs-content="Disabled popover">
                                <a :href="web + item.post_id" target="_blank" v-html="item.title"></a>
                            </span>
                           </td>
                        <td>{{item.area}} 坪</td>
                        <td>{{item.floor_str}}</td>
                        <td>{{item.kind_name}}</td>
                        <td>{{item.price}} {{item.price_unit}}</td>
                        <td v-html="item.location"></td>
                        <td>{{item.role_name}}</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div v-else-if="isCardMode">
            <label for="customRange" class="form-label">每排顯示筆數</label>
            <input type="range" class="form-range" min="2" max="4" v-model.number="rowCount" id="customRange">

            <div class="row row-cols-1 g-4" :class="returnColCss">
                <object-card v-for="(item,index) in filteredItems" :item="item" :index="index" :web="web"></object-card>
            </div>
        </div>

        <nav aria-label="Page navigation example">
            <ul class="pagination justify-content-center">
                <li class="page-item" :class="{'disabled':page === 0}"><a v-on:click="page = 0" class="page-link" href="#" tabindex="-1" aria-disabled="true">&laquo;</a></li>
                <li class="page-item" :class="{'disabled':page === 0}"><a v-on:click="page--" class="page-link" href="#" tabindex="-1" aria-disabled="true">&lt;</a></li>
                <li v-for="index in filteredPage" class="page-item" :class="{'active':page === index - 1}"><a v-on:click="page = index - 1" class="page-link" :name="index" href="#">{{index}}</a></li>
                <li class="page-item" :class="{'disabled':page === filteredPage - 1}"><a v-on:click="page++" class="page-link" href="#">&gt;</a></li>
                <li class="page-item" :class="{'disabled':page === filteredPage - 1}"><a v-on:click="page = filteredPage - 1" class="page-link" href="#">&raquo;</a></li>
            </ul>
        </nav>

    </div>

    <div v-else-if="error">
        <h2 style="color:red">{{error.name}}</h2>
        <p>{{error.message}}</p>
    </div>

    <div v-else>
        <h1>No Data</h1>
    </div>
        `
}