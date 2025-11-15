import { NormalItems } from "../interfaces/component-models";
import { Discount } from "../interfaces/discounts";
import { ExtendedProduct, ProductMeta } from "../interfaces/product-meta";

export const getProductsMeta = (products: ExtendedProduct[])=>{
    const result : ProductMeta[] = [];
    for(let item of products)
    {
        result.push(getProductMetaModel(item));
    }
    return result;
}

function getProductMetaModel(product: ExtendedProduct) : ProductMeta
{
    const discountPercentage = getDiscountInfo(product.discounts);
    return {
        id:product.id,
        name: product.name,
        image: product.variants?.[0].images[0],
        actualPrice: product.variants![0].price,
        discountedPrice: getDiscountAmount(discountPercentage, product.variants![0].price),
        rating: product.rating,
        isLiked: false,
        variantId: product.variants?.[0].id,
        data: product,
        discount: discountPercentage
    } as any;
}

function getDiscountAmount(perc: number, actualPrice: number)
{
    let val = actualPrice - (perc/100)*actualPrice;
    if(val < 0)
        return 0;
    else
    return val;
}

function getDiscountInfo(discounts: Discount[])
{
    let discountResult = 0;
    for(let item of discounts??[])
    {
        discountResult = Math.max(discountResult, item.percentage);
    }
    return discountResult;
}

export const getCategoryNormalItems = (items: string[])=>{
    var response : NormalItems[] = [];
    for(let item of items)
    {
        response.push(
        {
            id: 1,
            imageUrl: `https://placehold.co/60x60/cccccc/333333?text=${item}`,
            title: item
        } as any);
    }
    return response;
}