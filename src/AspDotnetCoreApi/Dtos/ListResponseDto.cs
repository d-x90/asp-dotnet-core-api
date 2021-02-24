using System.Collections.Generic;

namespace AspDotnetCoreApi.Dtos {
    public class ListResponseDto<T> {
        public List<T> Items { get; set; }
    }
}