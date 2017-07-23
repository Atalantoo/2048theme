import java.util.List;

import org.springframework.batch.item.ItemReader;
import org.springframework.batch.item.NonTransientResourceException;
import org.springframework.batch.item.ParseException;
import org.springframework.batch.item.UnexpectedInputException;

import lombok.NonNull;
import lombok.RequiredArgsConstructor;

@RequiredArgsConstructor
public class InMemoryReader<T> implements ItemReader<T> {
	@NonNull
	private List<T> inMemoryList;

	@Override
	public T read() throws Exception, UnexpectedInputException, ParseException, NonTransientResourceException {
		if (inMemoryList.isEmpty())
			return null;
		return inMemoryList.remove(0);
	}

}
